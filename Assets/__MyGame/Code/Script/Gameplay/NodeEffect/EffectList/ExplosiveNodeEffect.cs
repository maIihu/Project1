using __MyGame.Code.Script;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeEffect/ExplosiveNodeEffect")]
public class ExplosiveNodeEffect : NodeEffect, IChangeNodeSprite, IOnNodeEnter
{
	[SerializeField] private int damage;
	[SerializeField] private int radius;
	[SerializeField] BaseSkillEffect explodeEffect;
	private void OnEnable()
	{
		duration = -1;
	}
	public Sprite GetNodeEffectSprite(Node node, NodeEffectInstance inst, Sprite baseSprite)
	{
		return effectSpriteOverlay;
	}

	public void OnNodeEnter(BoardController board, TileEntity entity, Node node)
	{
		GameplayManager.Instance.RegisterPostMoveAction(() => Explode(board, node));
	}

	private UniTask Explode(BoardController board, Node node)
	{
		var nodePosition = node.GridPos;
		var explodePos = node.transform.position;
		var explode = Instantiate(explodeEffect, explodePos, Quaternion.identity);
		var effectInflicter  = node.NodeEffectChangeEntity;
		explode.Play();
		for (int dx = -radius; dx <= radius; dx++)
		{
			for (int dy = -radius; dy <= radius; dy++)
			{
				if (dx == 0 && dy == 0) continue;
				var n = board.GetNodeAtPosition(nodePosition + new Vector2(dx, dy));
				if (n != null && n.OccupiedEntity != null)
				{
					_ = n.OccupiedEntity.AnimateHit();
					n.OccupiedEntity.TakeDamage(damage,effectInflicter);
				}
			}
		}
		node.ClearEffect();
		return UniTask.CompletedTask;
	}
}
