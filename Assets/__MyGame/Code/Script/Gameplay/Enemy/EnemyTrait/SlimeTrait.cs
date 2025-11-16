using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeTrait", menuName = "Enemy/Trait/SlimeTrait")]
public class SlimeTrait : EnemyTrait, IOnDeath
{
	[SerializeField] private SlideNodeEffect slideEffect;
	[SerializeField] private int slideDuration = 3;
	[SerializeField] private int radius = 1;

	public void OnDeath(BoardController board, EnemyEntity self, Node node)
	{
		if (node == null) return;
		var c = node.GridPos;
		for (int dx = -radius; dx <= radius; dx++)
		for(int dy = -radius; dy <= radius; dy++)
		{
				if (dx == 0 && dy == 0) continue;
				var n = board.GetNodeAtPosition(c + new Vector2(dx, dy));
				if(n != null && n.OccupiedEntity != null)
				{
					n.OccupiedEntity.TakeDamage(self.attack);
				}
		}
		if(slideEffect != null)
		{
			node.AddEffect(slideEffect, slideDuration);
		}
	}
}
