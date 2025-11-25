using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ranger/Detonation Trap")]
public class DetonationTrap : BaseCharacterAbility
{
	[SerializeField] private ExplosiveNodeEffect explosiveEffect;
	private void OnEnable()
	{
		abilityName = "Detonation Trap";
		abilityType = AbilityType.Active;
		target = AbilityTarget.Node;
		phase = CastPhase.Instant;
		consumeTurn = false;
	}
	public override UniTask OnCast(PlayerEntity user, AbilityContext ctx)
	{
		var targetNode = ctx.targetNode;
		targetNode.AddEffect(explosiveEffect,-1,user);
		return UniTask.CompletedTask;
	}
}
