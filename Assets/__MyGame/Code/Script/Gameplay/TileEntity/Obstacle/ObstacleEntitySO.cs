using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleEntity", menuName = "ObstacleEntity")]
public class ObstacleEntitySO : ScriptableObject
{
	public string Name;
	public ObstacleType Type;
	public Sprite Sprite;
	public int Health;
	public bool BlockMovement;
}
