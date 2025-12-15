using __MyGame.Code.Script;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GiantBossTrait", menuName = "Enemy/Trait/GiantBossTrait")]
public class GiantBossTrait : EnemyTrait, IOnAfterMove
{
	[Header("Info")]
	public int radius;
	public int damage;
	public int cooldownTurns;

	private int currentCooldown;

	[Header("Animation")]
	[SerializeField] private BaseSkillEffect shockwavePrefab;
	[SerializeField] private float shockwaveDuration = 0.4f;

	private void OnEnable()
	{
		currentCooldown = cooldownTurns;
	}

	public void OnAfterMove(BoardController board, EnemyEntity self, Node from, Node to)
	{
		currentCooldown = Mathf.Max(0, currentCooldown - 1);

		if (currentCooldown > 0) return;

		var centerNode = to;
		if (centerNode == null) return;
		GameplayManager.Instance.RegisterPostMoveAction(
			() => PlayGiantAoESequence(board, self, centerNode)
		);
		currentCooldown = cooldownTurns;
	}
	private  UniTask PlayGiantAoESequence(BoardController board, EnemyEntity self, Node centerNode)
	{
		var shockwave = Instantiate(shockwavePrefab, centerNode.transform.position, Quaternion.identity);
		shockwave.Play();
		var center = centerNode.GridPos;
		foreach (var node in board.AllNode)
		{
			var pos = node.GridPos;
			int dx = (int)Mathf.Abs(pos.x - center.x);
			int dy = (int)Mathf.Abs(pos.y - center.y);
			if (dx + dy <= radius)
			{
				var target = node.OccupiedEntity;
				if (target != null && target != self)
				{
					target.TakeDamage(damage, self);
				}
			}
		}
		return UniTask.CompletedTask;
	}
}
