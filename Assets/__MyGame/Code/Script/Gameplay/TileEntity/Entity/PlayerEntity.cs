using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : TileEntity
{
	public CharacterClass characterClass;
	public Dictionary<BaseCharacterAbility,int> abilityCooldowns = new Dictionary<BaseCharacterAbility, int>();
	//public List<Effect> effects = new();

	public void CharacterInitial(CharacterClass characterClass)
	{
		this.characterClass = characterClass;
		SetSpriteRuntime(characterClass.classIcon);
		maxHP = currentHP = characterClass.baseHP;
		attack = characterClass.baseAtk;
		armor = characterClass.baseArmor;
		BlocksMovement = true;
		abilityCooldowns.Clear();
		moveStep = characterClass.moveStep;
		SyncWorldPosToGrid();
	}

	public bool CanUse(BaseCharacterAbility ability) => abilityCooldowns.ContainsKey(ability) == false || abilityCooldowns[ability] <= 0;

}
