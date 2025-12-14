using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Knight/Counter Attack")]
public class CounterAttack : BaseCharacterAbility
{
	public int deflectDamage = 1;

	public override bool CanCast(PlayerEntity user, AbilityContext ctx)
	{
		if (ctx.targetNode == null) return false;
		var target = ctx.targetNode.OccupiedEntity;
		if (target == null) return false;

		var dist = (Vector2)(target.transform.position - user.transform.position);
		return Mathf.Abs(dist.x) + Mathf.Abs(dist.y) <= 1.1f;
	}

	public override UniTask OnCast(PlayerEntity user, AbilityContext ctx)
	{
		var target = ctx.targetNode?.OccupiedEntity;
		if (target == null) return UniTask.CompletedTask;

		target.TakeDamage(user.attack + deflectDamage, user);
		return UniTask.CompletedTask;
	}

	private void OnEnable()
	{
		abilityName = "Counter Attack";
		abilityType = AbilityType.Passive;
		target = AbilityTarget.Instant;
		phase = CastPhase.Reaction;
		consumeTurn = false;
		cooldownTurns = 2;
	}
}
