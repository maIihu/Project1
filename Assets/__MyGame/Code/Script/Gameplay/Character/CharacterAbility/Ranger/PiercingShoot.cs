using __MyGame.Code.Script;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

	public override UniTask OnCast(PlayerEntity user, AbilityContext ctx)
	{
		GameplayManager.Instance.RegisterPostMoveAction(() => ShootArrow(user, ctx));
		return UniTask.CompletedTask;
	}

	private async UniTask ShootArrow(PlayerEntity user, AbilityContext ctx)
	{
		var board = ctx.board;
		var dir = ctx.direction;
		if (board  == null || user == null)
		{
			Debug.Log("Piercing shot board/user null");
			return;
		}
		var startNode = board.GetNodeWithEntity(user);
		if(startNode == null)
		{
			Debug.Log("User not on board");
			return;
		}
		Vector2 startGrid = startNode.GridPos;
		Vector3 startWorld = startNode.transform.position;

		var pathNodes = new List<Node>();
		for(int i = 1; i <= range; i++)
		{
			var probe = board.GetNodeAtPosition(startGrid + dir * i);
			if (probe == null) break;
			pathNodes.Add(probe);
		}
		if(pathNodes.Count == 0)
		{
			Debug.Log("No valid path for piercing shot");
			return;
		}

		var arrow = Object.Instantiate(arrowPrefab, startWorld, Quaternion.identity);
		if (dir == Vector2Int.right) arrow.transform.rotation = Quaternion.Euler(0, 0, 180);
		if(dir == Vector2Int.up) arrow.transform.rotation = Quaternion.Euler(0, 0, -90);
		if(dir == Vector2Int.down) arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
		float stepduration = 0.1f;
		foreach(var node in pathNodes)
		{
			Vector3 targetPost = node.transform.position;
			await arrow.transform.DOMove(targetPost,stepduration).
				SetEase(Ease.Linear).SetLink(arrow).AsyncWaitForCompletion();
			var target = node.OccupiedEntity;
			if(target != null && target != user) 
			{
				_ = target.AnimateHit();
				target.TakeDamage(user.attack + damage,user);
			}
		}
		Object.Destroy(arrow);

	}
	private void OnEnable()
	{
		phase = CastPhase.InsteadOfMove;
		target = AbilityTarget.Direction;
		consumeTurn = true;
	}

}
