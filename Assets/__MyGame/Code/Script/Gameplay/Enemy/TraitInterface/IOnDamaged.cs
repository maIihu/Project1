using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnDamaged 
{
	void OnDamaged(BoardController board, EnemyEntity self, TileEntity attacker, int damageTaken);
}
