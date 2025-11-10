using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Knight/Heavy Slash")]
public class HeavySlash : BaseCharacterAbility
{
	public int damage;
	public int radiusRange;

	private void OnEnable()
	{
		phase = CastPhase.AfterMove;
		consumeTurn = true;
	}

	public override void OnCast(PlayerEntity user, AbilityContext ctx)
	{
		
	}
}
