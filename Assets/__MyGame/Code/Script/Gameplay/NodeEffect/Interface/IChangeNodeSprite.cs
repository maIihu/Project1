using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChangeNodeSprite
{
	public Sprite GetNodeEffectSprite(Node node, NodeEffectInstance inst, Sprite baseSprite);
}
