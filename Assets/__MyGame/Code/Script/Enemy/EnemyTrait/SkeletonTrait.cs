using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonTrait", menuName = "Enemy/Trait/SkeletonTrait")]
public class SkeletonTrait : EnemyTrait, IOnDeath
{
	public void OnDeath(BoardController board, EnemyEntity self, Node node)
	{
		
	}
}
