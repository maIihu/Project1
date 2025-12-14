using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyTraits/Thorns")]
public class ThornyTrait : EnemyTrait, IOnDamaged
{
	public int reflectDamage = 1;
	public void OnDamaged(BoardController board, EnemyEntity self, TileEntity attacker, int damageTaken)
	{
		if (attacker == null) return;
		var dist = (Vector2)(attacker.transform.position - self.transform.position);
		if (Mathf.Abs(dist.x) + Mathf.Abs(dist.y) <= 1.1f)
		{
			attacker.TakeDamage(reflectDamage, self);
		}
	}
}
