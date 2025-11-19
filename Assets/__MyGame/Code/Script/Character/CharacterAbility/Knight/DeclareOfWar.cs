using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Knight/Declare of War")]
public class DeclareOfWar : BaseCharacterAbility
{
	public int damage = 1;
	public int radius = 1;

	private void OnEnable()
	{
		abilityName = "Declare of War";
		abilityType = AbilityType.Active;
		target = AbilityTarget.Node;
		phase = CastPhase.Instant;
		consumeTurn = false;
		cooldownTurns = 3;
	}

	public override bool CanCast(PlayerEntity user, AbilityContext ctx)
	{
		if(ctx.targetNode == null)
		{
			return false;
		}
		if(ctx.targetNode.OccupiedEntity != null)
		{
			return false;
		}
		return true;
	}

	public override async Task OnCast(PlayerEntity user, AbilityContext ctx)
	{
		var board = ctx.board;
		if(board == null || ctx.targetNode == null)
		{
			return; 
		}
		var fromNode = board.GetNodeWithEntity(user);
		var targetNode = ctx.targetNode;
		if(fromNode != null && ReferenceEquals(fromNode.OccupiedEntity, user))
		{
			fromNode.OccupiedEntity = null;
			await user.AnimateJump(fromNode.GridPos, targetNode.GridPos);
		}
		user.SyncWorldPosToGrid();
		targetNode.OccupiedEntity = user;
		var center = targetNode.GridPos;
		for (int dx = -radius; dx <= radius; dx++)
		{
			for (int dy = -radius; dy <= radius; dy++)
			{
				if (dx == 0 && dy == 0) continue;
				var probe = board.GetNodeAtPosition(center + new Vector2Int(dx, dy));
				if (probe == null) continue;
				var target = probe.OccupiedEntity;
				if (target != null)
				{
					_ = target.AnimateHit();
					//await target.AnimateHit();
					target.TakeDamage(damage);
				}
			}
		}
	}
	//public override IEnumerator OnCast(PlayerEntity user, AbilityContext ctx)
	//{
	//	var board = ctx.board;
	//	if (board == null || ctx.targetNode == null)
	//	{
	//		Debug.Log("Declare Of War bug");
	//		yield break;
	//	}
	//	var fromNode = board.GetNodeWithEntity(user);
	//	var targetNode = ctx.targetNode;
	//	if (fromNode != null && ReferenceEquals(fromNode.OccupiedEntity, user))
	//	{
	//		fromNode.OccupiedEntity = null;
	//		yield return user.AnimateJump(fromNode.GridPos, targetNode.GridPos);
	//		Debug.Log("Jumped");	
	//	}
	//	user.SyncWorldPosToGrid();
	//	targetNode.OccupiedEntity = user;
	//	var center = targetNode.GridPos;
	//	for(int dx = -radius; dx <= radius; dx++)
	//	{
	//		for(int dy = -radius; dy <= radius; dy++)
	//		{
	//			if(dx == 0 && dy == 0) continue;
	//			var probe = board.GetNodeAtPosition(center + new Vector2Int(dx, dy));
	//			if (probe == null) continue;
	//			var target = probe.OccupiedEntity;
	//			if(target != null)
	//			{
	//				yield return target.AnimateHit();
	//				target.TakeDamage(damage);
	//			}
	//		}
	//	}
	//	yield return null;
	//}

}
