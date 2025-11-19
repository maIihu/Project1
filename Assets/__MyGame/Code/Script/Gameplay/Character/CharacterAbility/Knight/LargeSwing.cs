//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(menuName = "Abilities/Knight/Large Swing")]
//public class LargeSwing : BaseCharacterAbility
//{
//	public int damage;
//	public int radius;

//	private void OnEnable()
//	{
//		abilityName = "Large Swing";
//		abilityType = AbilityType.Active;
//		target = AbilityTarget.Direction;
//		phase = CastPhase.AfterMove;
//		consumeTurn = true;
//	}
//	public override bool CanCast(PlayerEntity user, AbilityContext ctx)
//	{
//		return true;
//	}
//	public override IEnumerator OnCast(PlayerEntity user, AbilityContext ctx)
//	{
//		var board = ctx.board;
//		if (board == null || ctx.direction == null)
//		{
//			Debug.Log("Large Swing bug");
//			yield break;
//		}
//		var fromNode = board.GetNodeWithEntity(user);
//		if(ctx.direction == Vector2Int.right)
//		{
//			for(int i = -radius ;i <= radius; i++)
//			{
//				var targetedNode = fromNode.GridPos - new Vector2Int(1, i);
//				var probe = board.GetNodeAtPosition(targetedNode);
//				var targetedEntity = probe?.OccupiedEntity;
//				if(targetedEntity != null)
//				{
//					yield return targetedEntity.AnimateHit();
//					targetedEntity.TakeDamage(damage);
//				}
//			}
//		}
//		else if (ctx.direction == Vector2Int.left)
//		{
//			for (int i = -radius; i <= radius; i++)
//			{
//				var targetedNode = fromNode.GridPos + new Vector2Int(1, i);
//				var probe = board.GetNodeAtPosition(targetedNode);
//				var targetedEntity = probe?.OccupiedEntity;
//				if (targetedEntity != null)
//				{
//					yield return targetedEntity.AnimateHit();
//					targetedEntity.TakeDamage(damage);
//				}
//			}
//		}
//		else if (ctx.direction == Vector2Int.up)
//		{
//			for (int i = -radius; i <= radius; i++)
//			{
//				var targetedNode = fromNode.GridPos + new Vector2Int(i, -1);
//				var probe = board.GetNodeAtPosition(targetedNode);
//				var targetedEntity = probe?.OccupiedEntity;
//				if (targetedEntity != null)
//				{
//					yield return targetedEntity.AnimateHit();
//					targetedEntity.TakeDamage(damage);
//				}
//			}
//		}
//		else if (ctx.direction == Vector2Int.down)
//		{
//			for (int i = -radius; i <= radius; i++)
//			{
//				var targetedNode = fromNode.GridPos + new Vector2Int(i, 1);
//				var probe = board.GetNodeAtPosition(targetedNode);
//				var targetedEntity = probe?.OccupiedEntity;
//				if (targetedEntity != null)
//				{
//					yield return targetedEntity.AnimateHit();
//					targetedEntity.TakeDamage(damage);
//				}
//			}
//		}
//		yield return null;
//	}
//}
