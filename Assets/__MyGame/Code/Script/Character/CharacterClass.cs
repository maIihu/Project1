using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RogueSlide/Character Class")]
public class CharacterClass : ScriptableObject
{
	public string className;
	public Sprite classIcon;

	[Header("Base Stats")]
	public int baseHP;
	public int baseAtk;
	public int baseArmor;
	public int moveStep;
	[Header("Ability")]
	public List<BaseCharacterAbility> abilities = new();
	
	

}
