using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnAfterMove 
{
	public void OnAfterMove(BoardController board, EnemyEntity self, Node from, Node to);
}
