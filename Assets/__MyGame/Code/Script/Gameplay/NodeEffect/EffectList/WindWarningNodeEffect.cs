using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindWarningNodeEffect : NodeEffect, IChangeNodeSprite
{
	public Sprite GetNodeEffectSprite(Node node, NodeEffectInstance inst, Sprite baseSprite)
	{
		return effectSpriteOverlay;
	}

}
