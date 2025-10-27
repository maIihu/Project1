using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : TileEntity
{
	public EmemyType enemyType;
	//public List<Effecst> effects 

	public void EmemyInit(EmemyType enemyType)
	{
		this.enemyType = enemyType;
		SetSpriteRuntime(enemyType.sprite);
		maxHP = currentHP = enemyType.maxHP;
		attack = enemyType.attack;
		armor = enemyType.armor;
		BlocksMovement = enemyType.blocksMovement;
		moveStep = enemyType.moveStep;
		SyncWorldPosToGrid();
	}
}
