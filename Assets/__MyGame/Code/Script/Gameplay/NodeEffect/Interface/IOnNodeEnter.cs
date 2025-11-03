using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnNodeEnter 
{
	public void OnNodeEnter(BoardController board, TileEntity entity, Node node);
}
