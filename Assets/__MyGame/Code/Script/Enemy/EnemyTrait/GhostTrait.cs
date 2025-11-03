using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostTrait", menuName = "Enemy/Trait/GhostTrait")]
public class GhostTrait : EnemyTrait, IGhostMove
{
	[SerializeField] private int hpLossPerMove;

	public void OnAfterMove(BoardController board, EnemyEntity self, Node from, Node to)
	{
		bool moved = from != null && to != null && from != to;
		if (moved)
		{
			self.TakeDamage(hpLossPerMove);
			if (self.currentHP <= 0) return;
		}

		var target = to?.OccupiedEntity;
		if (target != null && target != self)
		{
			target.TakeDamage(self.currentHP);
			self.TakeDamage(self.currentHP);
		}
	}
}
