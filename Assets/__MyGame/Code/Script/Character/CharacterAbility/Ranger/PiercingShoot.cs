using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Abilities/Ranger/Piercing Shot")]
public class PiercingShoot : BaseCharacterAbility
{
	public int range = 6;
	public int damage = 2;

	private void OnEnable()
	{
		phase = CastPhase.InsteadOfMove;
		consumeTurn = true;
	}
	public override void OnCast(PlayerEntity user, AbilityContext ctx)
	{
		var board =  ctx.board;
		var dir = ctx.direction;
		var node = board.GetNodeAtPosition(user.transform.position);
		for (int i = 1; i <= range; i++)
		{
			var probe = board.GetNodeAtPosition(node.GridPos + dir * i);
			if (probe == null) break;

			if (probe.OccupiedEntity is ObstacleEntity) break;

			var target = probe.OccupiedEntity;
			if (target != null && target != user)
				target.TakeDamage(user.attack + damage);
		}
	}
}
