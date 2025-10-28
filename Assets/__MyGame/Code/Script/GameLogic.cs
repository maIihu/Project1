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
			cur.OccupiedBlock = null;
			int step = Mathf.Max(1, ent.moveStep);
			var next = cur;
			while (step-- > 0)
			{
				var probe = _board.GetNodeAtPosition(next.GridPos + dir);
				if (!probe) break;
				var blocker = probe.OccupiedBlock;
				if (blocker != null)
				{
					blocker.TakeDamage(ent.attack);

					if (blocker.currentHP <= 0)
					{
						next = probe;
						continue;
					}
					else
					{
						break;
					}
				}
				next = probe;
			}
			next.OccupiedBlock = ent;
			ent.transform.position = next.GridPos;
			ent.SyncWorldPosToGrid();
		}
	}
}