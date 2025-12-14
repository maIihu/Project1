using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeEffect/WindNodeEffect")]
public class WindNodeEffect : NodeEffect, IModifyDirection, IChangeNodeSprite
{
	public Sprite GetNodeEffectSprite(Node node, NodeEffectInstance inst, Sprite baseSprite)
	{
		return effectSpriteOverlay;
	}

	public void ModifyDirection(ref Vector2 dir, BoardController board, TileEntity ent, Node fromNode)
	{
		dir = -dir;
	}
}
