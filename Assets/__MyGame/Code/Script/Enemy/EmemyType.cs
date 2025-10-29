using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType",menuName = "RougueSlide/EnemyType")]
public class EmemyType : ScriptableObject
{
	public string enemyName;
	public Sprite sprite;
	[Header("Stats")]
	public int maxHP;
	public int attack;
	public int armor;
	public int moveStep;

	[Header("Traits")]
	public bool blocksMovement;
	public List<EnemyTrait> enemyTraits;

}
