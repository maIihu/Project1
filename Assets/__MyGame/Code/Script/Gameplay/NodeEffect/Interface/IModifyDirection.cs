using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifyDirection 
{
	void ModifyDirection(ref Vector2 dir, BoardController board, TileEntity ent, Node fromNode);
}
