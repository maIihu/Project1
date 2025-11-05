using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class GameLogic
    {
        private BoardController _board;

        public GameLogic(BoardController board)
        {
            _board = board;
        }


		public void Shift(Vector2 dir)
		{
			var ents = OrderEntitiesByDirection(_board.GetAllEntities(), dir);
			foreach (var ent in ents) {
				Move(ent, dir);
				Debug.Log($"{ ent.name} moved to { ent.transform.position}");
			}

				foreach (var node in _board.AllNode)
			{
				node.ReduceExistTurn();
			}
		}

		private static bool IsGhost(TileEntity ent) => ent is EnemyEntity ee && ee.HasTrait<IGhostMove>();

		private static int GetMoveLayer(TileEntity e)
		{
			if (IsGhost(e)) return 2;
			if (e is EnemyEntity) return 1;
			if(e is PlayerEntity) return 0;
			return 1;
		}

		private static Vector2Int GridOf(TileEntity e)
		{
			var p = e.transform.position;
			return new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y));
		}

		private static List<TileEntity> OrderEntitiesByDirection(List<TileEntity> ents, Vector2 dir)
		{
			return ents
	.Where(e => e != null)
	.Select(e =>
	{
		var g = GridOf(e);
		int layer = GetMoveLayer(e);
		int project = g.x * (int)dir.x + g.y * (int)dir.y;
		int orth = g.x * (int)dir.y - g.y * (int)dir.x;
		int id = e.GetInstanceID();
		return new { e, layer, project, orth, id };
	})
	.OrderBy(k => k.layer)               
	.ThenByDescending(k => k.project)    
	.ThenBy(k => k.orth)               
	.ThenBy(k => k.id)                   
	.Select(k => k.e)
	.ToList();
		}

		public void Move(TileEntity ent, Vector2 dir)
		{
			bool isGhost = ent is EnemyEntity ee && ee.HasTrait<IGhostMove>();

			var fromNode = isGhost
			? _board.GetNodeAtPosition(ent.transform.position)
			: _board.GetNodeWithEntity(ent);
			if (!fromNode) return;

			if (!isGhost) fromNode.OccupiedEntity = null;
			var nextNode = FindNextNode(ent, fromNode, dir,isGhost);
			
			var moved = nextNode != fromNode;

			if (!isGhost) nextNode.OccupiedEntity = ent;
			ent.transform.position = nextNode.GridPos;
			ent.SyncWorldPosToGrid();
			var inst = nextNode.nodeEffect;
			if(inst != null && inst.effect is IOnNodeEnter onEnter)
			{
				Debug.Log($"Entity {ent.name} entered node at {nextNode.GridPos} with effect {inst.effect.GetType().Name}");
				onEnter.OnNodeEnter(_board, ent, nextNode);
			}

			if (ent is EnemyEntity enemy)
				enemy.RaiseAfterMove(_board, fromNode, nextNode);

		}

		private Node FindNextNode(TileEntity ent, Node fromNode, Vector2 dir,bool isGhost)
		{
			int stepLeft = ent.moveStep;
			var nextNode = fromNode;
			bool slideLatched = false;

			while (true)
			{
				bool forceSlideContinues = false;
				
				ApplyNodeEffects(ent, nextNode, ref stepLeft, ref slideLatched, ref forceSlideContinues);
				
				if(!forceSlideContinues && !slideLatched && stepLeft-- <= 0)
					break;

				var probeNode = _board.GetNodeAtPosition(nextNode.GridPos + dir);
				if(!probeNode) break;

				if (!isGhost && HandleBlocker(ent, probeNode)) break;

				var effectNext = probeNode.nodeEffect?.effect; 
				if (effectNext is SlideNodeEffect) 
					slideLatched = true; 
				
				nextNode = probeNode;
			}

			return nextNode;
		}

		private void ApplyNodeEffects(TileEntity ent, Node node, 
			ref int stepLeft, ref bool slideLatched, ref bool forceSlideContinues)
		{
			var effect = node.nodeEffect?.effect;
			if (effect is SlideNodeEffect) slideLatched = true;
			if(effect is IModifyMovement mod)
				mod.ModifyMovement(ref stepLeft, ref forceSlideContinues, _board, ent, node);
		}

		private bool HandleBlocker(TileEntity ent, Node probeNode)
		{
			var blocker = probeNode.OccupiedEntity;
			if (!blocker) return false;
			blocker.TakeDamage(ent.attack);
			return true;
		}
		
    }
}