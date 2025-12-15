using System.Collections;
using System.Collections.Generic;
using __MyGame.Code.Script.Gameplay.Enemy.ExpBall;
using __MyGame.Code.Script.Helper;
using DG.Tweening;
using UnityEngine;

public class PlayerEntity : TileEntity, ILevelUpAble
{
	public CharacterClass characterClass;
	private readonly HashSet<BaseCharacterAbility> learnedAbilities = new HashSet<BaseCharacterAbility>();
	public Dictionary<BaseCharacterAbility,int> cooldowns = new Dictionary<BaseCharacterAbility, int>();

	public IEnumerable<BaseCharacterAbility> LearnedAbilities => learnedAbilities;
	public int level { get; private set; } = 1;
	public int currentExp { get; private set; } = 0;

	[SerializeField] private int baseExpToLevel = 100;
	[SerializeField] private float expGrowthRate = 1.5f;
	[SerializeField] private DamageFlashController damageFlash;
	[SerializeField] private ExpBallControl expBall;
	public int ExpToNextLevel => Mathf.FloorToInt(baseExpToLevel * Mathf.Pow(expGrowthRate, level - 1));

	public event System.Action<int> OnLevelChanged;
	public event System.Action<int,int> OnExpChanged;

	public event System.Action<BaseCharacterAbility> OnAbilityLearned;
	public event System.Action<BaseCharacterAbility,int> OnAbilityCooldownChanged;	
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
		moveStep = characterClass.moveStep;
		SyncWorldPosToGrid();

		level = 1;
		currentExp = 0;
		OnExpChanged?.Invoke(currentExp, ExpToNextLevel);

		learnedAbilities.Clear();
		cooldowns.Clear();

		UnlockSkillsForCurrentLevel();
		statView.InitView();
		expBall.Init();
	}

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

		UnlockSkillsForCurrentLevel();
	}
	private void UnlockSkillsForCurrentLevel()
	{
		if (characterClass == null || characterClass.abilities == null) return;

		foreach(var slot in characterClass.abilities)
		{
			if (slot.ability == null) continue;
			if (slot.unlockLevel > level) continue;

			if (learnedAbilities.Add(slot.ability))
			{
				OnAbilityLearned?.Invoke(slot.ability);
			}
		}
	}

	//CoolDown
	public bool IsAbilityUnlocked(BaseCharacterAbility ability)
		=> ability != null && learnedAbilities.Contains(ability);
	public bool CanUse(BaseCharacterAbility ability)
	{
		if (ability == null) return false;
		if (!IsAbilityUnlocked(ability)) return false;
		return !cooldowns.TryGetValue(ability, out int cd) || cd <= 0;
	}
	public void StartCooldown(BaseCharacterAbility ability)
	{
		if (ability == null) return;
		int cd = Mathf.Max(ability.cooldownTurns, 0);

		cooldowns[ability] = cd;
		OnAbilityCooldownChanged?.Invoke(ability, cd);
	}

	public void TickCooldowns()
	{
		var keys = new List<BaseCharacterAbility>(cooldowns.Keys);
		foreach (var ab in keys)
		{
			int old = cooldowns[ab];
			if (old <= 0) continue;
			int nu = old - 1;
			cooldowns[ab] = nu;
			OnAbilityCooldownChanged?.Invoke(ab, nu);
		}
	}

	public int GetCooldown(BaseCharacterAbility ability)
	{
		return cooldowns.TryGetValue(ability, out int cd) ? cd : 0;
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
