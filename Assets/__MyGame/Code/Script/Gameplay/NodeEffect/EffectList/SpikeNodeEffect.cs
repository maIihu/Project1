using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeEffect/SpikeNodeEffect")]
public class SpikeNodeEffect : CycledNodeEffect
{
	public float percentageSpikeDamage;
	public override void OnNodeEnterEffect(BoardController board, TileEntity entity, Node node)
	{
		entity.TakeDamage(Mathf.CeilToInt(entity.maxHP * percentageSpikeDamage));
	}
}
