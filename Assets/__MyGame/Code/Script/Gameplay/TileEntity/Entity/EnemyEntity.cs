using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyEntity : TileEntity
{
	public EmemyType enemyType;
	private List<EnemyTrait> traits = new();

	IOnAfterMove[] afterMoveHooker;
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

		traits = enemyType.enemyTraits;
		this.OnDied += HandleDeathTraits;
		afterMoveHooker = traits.OfType<IOnAfterMove>().ToArray();

	}
	private void HandleDeathTraits(TileEntity entity)
	{
		var board = BoardController.Instance;
		var node = board.GetNodeWithEntity(this);
		if (traits != null)
		{
			foreach (var t in traits)
			{
				if (t is IOnDeath deathTrait)
				{
					deathTrait.OnDeath(board, this, node);
				}
			}
		}
	}
	public void RaiseAfterMove(BoardController board, Node from, Node to)
	{
		foreach(var h in afterMoveHooker)
		{
			h.OnAfterMove(board, this, from, to);
		}
	}
}
