using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityContext
{
	public BoardController board;
	public GameLogic gameLogic;
	public Vector2Int direction;
	public Node targetNode;
}
