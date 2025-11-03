using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeEffect/SlideNodeEffect")]
public class SlideNodeEffect : NodeEffect, IModifyMovement, IChangeNodeSprite
{
	public Sprite GetNodeEffectSprite(Node node, NodeEffectInstance inst, Sprite baseSprite)
	{
		return effectSpriteOverlay;
	}

	public void ModifyMovement(ref int stepLeft, ref bool forceSlideContinue, BoardController board, TileEntity ent, Node node)
	{
		forceSlideContinue = true;
	}
}
