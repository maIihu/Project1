using __MyGame.Code.Script;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Knight/Large Swing")]
public class LargeSwing : BaseCharacterAbility
{
	public int damage;
	public int radius;
	[SerializeField] private LargeSwingSkillEffect swingEffectPrefab;

	private void OnEnable()
	{
		abilityName = "Large Swing";
		abilityType = AbilityType.Active;
		target = AbilityTarget.Direction;
		phase = CastPhase.AfterMove;
		consumeTurn = true;
	}
	public override bool CanCast(PlayerEntity user, AbilityContext ctx)
	{
		return true;
	}
	public override UniTask OnCast(PlayerEntity user, AbilityContext ctx)
	{
		GameplayManager.Instance.RegisterPostMoveAction(() => SwordSwing(user, ctx));
		return UniTask.CompletedTask;
	}

	private async UniTask SwordSwing(PlayerEntity user, AbilityContext ctx)
	{
		var board = ctx.board;
		var dir = ctx.direction;

		var userPos = board.GetNodeWithEntity(user).GridPos;
		Vector3 slashPos = user.transform.position;
		var slash = Instantiate(swingEffectPrefab, slashPos, Quaternion.identity);
		slash.Play(dir);
		Node targetNode = null;
		for (int i = -radius; i <= radius; i++)
		{
			if (dir == Vector2Int.left)
			{
				targetNode = board.GetNodeAtPosition(userPos + new Vector2Int(-1, i));
			}
			else if (dir == Vector2Int.right)
			{
				targetNode = board.GetNodeAtPosition(userPos + new Vector2Int(1, i));
			}
			else if (dir == Vector2Int.up)
			{
				targetNode = board.GetNodeAtPosition(userPos + new Vector2Int(i, 1));
			}
			else
			{
				targetNode = board.GetNodeAtPosition(userPos + new Vector2Int(i, -1));
			}
			var target = targetNode.OccupiedEntity;
			if(target != null && !ReferenceEquals(target, user))
			{
				await target.AnimateHit();	
				target.TakeDamage(user.attack + damage,user);
			}
		}
	}
}
