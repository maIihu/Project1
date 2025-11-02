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
			foreach (var ent in ents) Move(ent, dir);

			foreach(var node in _board.AllNode)
			{
				node.ReduceExistTurn();
			}
		}

		private static List<TileEntity> OrderEntitiesByDirection(List<TileEntity> ents, Vector2 dir)
		{
			if (dir == Vector2.right) return ents.OrderByDescending(e => e.transform.position.x).ToList();
			if (dir == Vector2.left) return ents.OrderBy(e => e.transform.position.x).ToList();
			if (dir == Vector2.up) return ents.OrderByDescending(e => e.transform.position.y).ToList();
			return ents.OrderBy(e => e.transform.position.y).ToList();
		}

		public void Move(TileEntity ent, Vector2 dir)
		{
			var fromNode = _board.GetNodeWithEntity(ent);
			if (!fromNode) return;

			fromNode.OccupiedEntity = null;
			var nextNode = FindNextNode(ent, fromNode, dir);
			
			var moved = nextNode != fromNode;
			
			nextNode.OccupiedEntity = ent;
			ent.transform.position = nextNode.GridPos;
			ent.SyncWorldPosToGrid();
			
			if(ent is EnemyEntity enemy && moved)
				enemy.RaiseAfterMove(_board, fromNode, nextNode);
		}

		private Node FindNextNode(TileEntity ent, Node fromNode, Vector2 dir)
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

				if(HandleBlocker(ent, probeNode)) break;
				
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