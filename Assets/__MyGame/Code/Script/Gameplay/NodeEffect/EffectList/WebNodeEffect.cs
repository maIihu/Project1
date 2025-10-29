using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "NodeEffect/WebNodeEffect")]
public class WebNodeEffect : NodeEffect, IModifyMovement
{
	public bool consumeOnHold = true;

	public void ModifyMovement(ref int stepLeft, ref bool forceSlideContinue, BoardController board, TileEntity ent, Node node)
	{
		stepLeft = 0;
		forceSlideContinue = false;
		if(consumeOnHold)
		{
			node.ClearEffect();
		}
	}
}
