using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifyMovement
{
	public void ModifyMovement(ref int stepLeft,ref bool forceSlideContinue,BoardController board, TileEntity ent, Node node);
}
