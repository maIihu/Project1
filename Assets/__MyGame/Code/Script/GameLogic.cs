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
			var cur = _board.GetNodeWithEntity(ent);
			if (cur == null) return;
			var fromNode = cur;
			cur.OccupiedEntity = null;
			int stepLeft = Mathf.Max(1, ent.moveStep);
			var next = cur;
			bool slideLatched = false;
			while (true)
			{
				bool forceSlideContinue = false;
				var effectInst = next.nodeEffect;
				var effect = effectInst?.effect;
				if (effect is SlideNodeEffect) slideLatched = true;
				if(effect is IModifyMovement mod)
				{
					mod.ModifyMovement(ref stepLeft, ref forceSlideContinue, _board, ent, next);
				}
				if (!forceSlideContinue  && !slideLatched)
				{
					if (stepLeft-- <= 0) break;
				}
				var probe = _board.GetNodeAtPosition(next.GridPos + dir);
				if (!probe) break;
				var blocker = probe.OccupiedEntity;
				if(blocker != null)
				{
					blocker.TakeDamage(ent.attack);
					if (blocker.currentHP > 0) break;
				}
				var effectNext = probe.nodeEffect?.effect;
				if (effectNext is SlideNodeEffect) slideLatched = true;
				next = probe;
			}
			next.OccupiedEntity = ent;
			ent.transform.position = next.GridPos;
			ent.SyncWorldPosToGrid();
			//place holder for enemy only first
			if(ent is EnemyEntity enemyEntity)
			{
				enemyEntity.RaiseAfterMove(_board, fromNode, next);
			}
		}
	}
}