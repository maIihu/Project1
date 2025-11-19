using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ranger/Piercing Shot")]
public class PiercingShoot : BaseCharacterAbility
{
	public int range = 6;
	public int damage = 2;
	public GameObject arrowPrefab;

	public override async Task OnCast(PlayerEntity user, AbilityContext ctx)
	{
		var board = ctx.board;
		var dir = ctx.direction;
		var node = board.GetNodeWithEntity(user);
		for(int i = 1; i<= range; i++)
		{
			var probe = board.GetNodeAtPosition(node.GridPos + dir * i);
			if (probe == null) break;
			if(probe.OccupiedEntity is ObstacleEntity) continue;
			var target = probe.OccupiedEntity;
			if(target != null && target != user)
			{
				_= target.AnimateHit();
				target.TakeDamage(user.attack + damage);
			}
		}
	}

	private void OnEnable()
	{
		phase = CastPhase.InsteadOfMove;
		target = AbilityTarget.Direction;
		consumeTurn = true;
	}
	//public override IEnumerator OnCast(PlayerEntity user, AbilityContext ctx)
	//{
	//	var board = ctx.board;
	//	var dir = ctx.direction;
	//	var node = board.GetNodeAtPosition(user.transform.position);

	//	for (int i = 1; i <= range; i++)
	//	{
	//		var probe = board.GetNodeAtPosition(node.GridPos + dir * i);
	//		if (probe == null) break;

	//		if (probe.OccupiedEntity is ObstacleEntity) continue;

	//		var target = probe.OccupiedEntity;
	//		if (target != null && target != user)
	//			target.TakeDamage(user.attack + damage);
	//	}
	//	yield return null;
	//}

}
