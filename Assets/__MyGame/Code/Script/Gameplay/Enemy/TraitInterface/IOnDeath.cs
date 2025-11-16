using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnDeath
{
	public void OnDeath(BoardController board,EnemyEntity self, __MyGame.Code.Script.Node node);
}
