using System;
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
	private Animator animator;

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
	}
	public void EnemyInit(EmemyType type)
	{
		this.enemyType = type;
		SetSpriteRuntime(type.sprite);
		maxHP = currentHP = type.maxHP;
		attack = type.attack;
		armor = type.armor;
		BlocksMovement = type.blocksMovement;
		moveStep = type.moveStep;
		SyncWorldPosToGrid();

		traits = type.enemyTraits;
		this.OnDied += HandleDeathTraits;
		afterMoveHooker = traits.OfType<IOnAfterMove>().ToArray();

		if(animator && type.animatorController)
		{
			animator.runtimeAnimatorController = type.animatorController;
		}
		if(enemyType.portrait)
		{
			entityPortrait = enemyType.portrait;
		}

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

	private void OnDisable()
	{
		this.OnDied -= HandleDeathTraits;
	}

	public void RaiseAfterMove(BoardController board, Node from, Node to)
	{
		foreach(var h in afterMoveHooker)
		{
			h.OnAfterMove(board, this, from, to);
		}
	}
	public bool HasTrait<T>() where T : class
	{
		if (traits == null) return false;
		for(int i = 0; i < traits.Count; i++)
		{
			if(traits[i] is T) return true;
		}
		return false;
	}
	#region
	public void AE_DeathImpact()
	{
		var board = BoardController.Instance;
		var node = board.GetNodeWithEntity(this);
		if(traits != null)
		{
			foreach(var t in traits)
			{
				if(t is IOnDeath deathTrait)
				{
					deathTrait.OnDeath(board, this, node);
				}
			}
		}
	}
	public void AE_DeathEnd()
	{
		CompleteDeath();
	}
	private void CompleteDeath()
	{
		Destroy(gameObject);
	}
	protected override IEnumerator DeathRoutine()
	{
		_isDead = true;

		BlocksMovement = false;

		if (animator)
		{
			animator.SetTrigger("Die");
			yield return new WaitForSeconds(1f);
			CompleteDeath();
		}
		else
		{
			yield return null;
			CompleteDeath();
		}
	}
	#endregion

}
