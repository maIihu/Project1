using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBossTrait : EnemyTrait, IOnEnemyTratCoolDown, IOnAfterMove
{
	public int radius;

	public int damage;
	public int cooldown;
	public bool CanUse()
	{
		if (cooldown <= 0) return true;
		return false;
	}

	public void OnAfterMove(BoardController board, EnemyEntity self, Node from, Node to)
	{
		var centerNode = to;
	}

	public void ReduceCoolDown()
	{
		cooldown--;
	}
}
