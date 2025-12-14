using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MapEffects/Red Spike Nodes")]
public class RedMapSpikeNodeEffect : MapEffect
{
	public SpikeNodeEffect spikeEffect;
	public override void Apply(BoardController board, MapData mapData)
	{
		if (mapData != null && mapData.mapType != MapType.Red) return;

		var nodes = board.AllNode;
		if (nodes == null || nodes.Count == 0) return;

		int want = Mathf.Clamp(Random.Range(5, 8), 0, nodes.Count);

		var picked = new HashSet<int>();
		int safety = 0;
		while (picked.Count < want && safety < nodes.Count * 10)
		{
			picked.Add(Random.Range(0, nodes.Count));
			safety++;
		}

		foreach (int idx in picked)
		{
			var node = nodes[idx];
			if (node == null) continue;

			if (node.nodeEffect != null) continue;

			node.AddEffect(spikeEffect, spikeEffect.duration);
		}
	}
}

