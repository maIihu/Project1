using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NodeEffect/SlideNodeEffect")]
public class SlideNodeEffect : NodeEffect, IModifyMovement
{
	public void ModifyMovement(ref int stepLeft, ref bool forceSlideContinue, BoardController board, TileEntity ent, Node node)
	{
		forceSlideContinue = true;
	}
}
