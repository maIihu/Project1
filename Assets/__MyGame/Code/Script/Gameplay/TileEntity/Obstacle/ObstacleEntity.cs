using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEntity : TileEntity
{
	public ObstacleEntitySO obstacleData;

	public void InitialObstacle(ObstacleEntitySO obstacle)
	{
		obstacleData = obstacle;
		maxHP = currentHP = obstacle.Health;
		attack = 0;
		armor = 0;
		BlocksMovement = obstacle.BlockMovement;
		moveStep = 0;
		SetSpriteRuntime(obstacle.Sprite);
	}
}
