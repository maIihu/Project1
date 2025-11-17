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
		if (to == null || self == null) return;

		bool moved = (to != from);
		if (moved)
		{
			//self.currentHP = Mathf.Max(0, self.currentHP - hpLossPerMove);
			//self.RefreshUI();
			self.TakeDamage(hpLossPerMove);
		}
		var target = to.OccupiedEntity;
		if(target == null)
		{
			//Debug.Log("Null");
		}
		if (target != null && target != self)
		{
			int dmg = Mathf.Max(1, self.currentHP);
			target.TakeDamage(dmg);
			self.TakeDamage(Mathf.Max(1, self.currentHP));
			return;
		}
		if (self.currentHP <= 0)
			self.TakeDamage(1);
	}
}
