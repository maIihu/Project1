using System.Collections;
using System.Collections.Generic;
using __MyGame.Code.Script.Gameplay.Enemy.ExpBall;
using __MyGame.Code.Script.Helper;
using DG.Tweening;
using UnityEngine;

public class PlayerEntity : TileEntity, ILevelUpAble
{
	public CharacterClass characterClass;
	public Dictionary<BaseCharacterAbility,int> abilities = new Dictionary<BaseCharacterAbility, int>();
	public int level { get; private set; } = 1;
	public int currentExp { get; private set; } = 0;

	[SerializeField] private int baseExpToLevel = 100;
	[SerializeField] private float expGrowthRate = 1.5f;
	[SerializeField] private DamageFlashController damageFlash;
	[SerializeField] private ExpBallControl expBall;
	public int ExpToNextLevel => Mathf.FloorToInt(baseExpToLevel * Mathf.Pow(expGrowthRate, level - 1));

	public event System.Action<int> OnLevelChanged;
	public event System.Action<int,int> OnExpChanged;

	public void CharacterInitial(CharacterClass characterClass)
	{
		this.characterClass = characterClass;
		SetSpriteRuntime(characterClass.classIcon);
		maxHP = currentHP = characterClass.baseHP;
		attack = characterClass.baseAtk;
		armor = characterClass.baseArmor;
		BlocksMovement = true;
		if(characterClass.classPortrait)
		{
			entityPortrait = characterClass.classPortrait;
		}
		abilities.Clear();
		moveStep = characterClass.moveStep;
		SyncWorldPosToGrid();

		level = 1;
		currentExp = 0;
		OnExpChanged?.Invoke(currentExp, ExpToNextLevel);
		
		statView.InitView();
		expBall.Init();
	}

	public bool CanUse(BaseCharacterAbility ability) => abilities.ContainsKey(ability) == false || abilities[ability] <= 0;

	public void GainExp(int exp, Vector3 sourcePos)
	{
		expBall.Play(sourcePos);
		if(exp <= 0) return;
		currentExp += exp;
		while (currentExp >= ExpToNextLevel)
		{
			currentExp -= ExpToNextLevel;
			LevelUp();
		}
		OnExpChanged?.Invoke(currentExp, ExpToNextLevel);
		//Debug.Log($"player gained {exp} exp point");
	}

	public void LevelUp()
	{
		level++;
		int hpGained = Mathf.RoundToInt(characterClass.baseHP * 0.2f);
		maxHP += hpGained;
		OnLevelChanged?.Invoke(level);
	}

	public override void TakeDamage(int damage, TileEntity attacker = null)
	{
		base.TakeDamage(damage, attacker);
		//damageFlash.StartFlash();
		var ft = FloatingTextPool.Instance.Get();
		ft.Play("-" + damage, this.transform.position);
		CameraManager.Instance.Shake();
	}


}
