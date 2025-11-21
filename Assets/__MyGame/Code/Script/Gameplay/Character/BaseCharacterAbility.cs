using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacterAbility : ScriptableObject
{
	[Header("Info")]
	public string abilityName;
	public AbilityType abilityType;
	public AbilityTarget target;
	public Sprite abilityIcon;

	public int cooldownTurns;

	[Header("Flow")]
	public CastPhase phase;
	public bool consumeTurn;

	public virtual bool CanCast(PlayerEntity user, AbilityContext ctx) => true;

	public abstract UniTask OnCast(PlayerEntity user, AbilityContext ctx);
}
