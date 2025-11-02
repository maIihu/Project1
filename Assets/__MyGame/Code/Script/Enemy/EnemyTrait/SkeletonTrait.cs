using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonTrait", menuName = "Enemy/Trait/SkeletonTrait")]
public class SkeletonTrait : EnemyTrait, IOnDeath
{
	[SerializeField] private ObstacleEntity ObstaclePrefab;
	[SerializeField] private ObstacleEntitySO ObstacleEntitySO;
	public void OnDeath(BoardController board, EnemyEntity self, Node node)
	{
		if(!node) return;
		var obs = board.InstantiateObstacleEntityAtNode(ObstaclePrefab, node);
		if (obs && ObstacleEntitySO) obs.InitialObstacle(ObstacleEntitySO);
	}
}
