using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTraitRuntime
{
	public EnemyTrait definition { get; }
	public EnemyEntity owner { get; }

	protected EnemyTraitRuntime(EnemyTrait definition, EnemyEntity owner)
	{
		this.definition = definition;
		this.owner = owner;
	}
}
