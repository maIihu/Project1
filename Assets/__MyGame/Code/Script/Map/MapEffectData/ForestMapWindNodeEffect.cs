using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "MapEffects/Forest Wind Nodes")]
public class ForestMapWindNodeEffect : MapEffect
{
	public WindNodeEffect windEffect;

	public override void Apply(BoardController board, MapData mapData)
	{
		if (board == null || windEffect == null) return;

		int size = BoardController.BoardSize;
		int offset = size / 2;

		int min = -offset;
		int maxExclusive = offset;

		var rng = new System.Random();

		bool pickRow = rng.Next(0, 2) == 0;
		int line = rng.Next(min, maxExclusive);

		foreach (var node in board.AllNode)
		{
			if (node == null) continue;

			int x = Mathf.RoundToInt(node.GridPos.x);
			int y = Mathf.RoundToInt(node.GridPos.y);

			bool hit = pickRow ? (y == line) : (x == line);
			if (hit)
			{
				node.AddEffect(windEffect, windEffect.duration);
			}
		}
	}
}
