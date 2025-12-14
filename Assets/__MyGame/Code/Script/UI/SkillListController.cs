using __MyGame.Code.Script;
using _MyCore.DesignPattern.Observer.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListController : MonoBehaviour, IMessageHandle
{
	[SerializeField] private PassiveSkillUI passiveSkillPrefab;
	[SerializeField] private ActiveSkillButtonUI skillButtonPrefab;
	[SerializeField] private Transform skillListContainer;

	private readonly Dictionary<BaseCharacterAbility, ActiveSkillButtonUI> skillButtonUIs = new Dictionary<BaseCharacterAbility, ActiveSkillButtonUI>();
	private ActiveSkillButtonUI currentSelected;
	private PlayerEntity player;
	public void Handle(Message message)
	{
		switch(message.Type)
		{
			case ProjectMessageType.OnActiveskillSelected:
				OnSkillSelected();
				break;
			case ProjectMessageType.OnActivesSkillCancled:
				break;
		}
	}
	private void OnSkillSelected()
	{
	}

	public void Clear()
	{
		foreach(Transform t in skillListContainer)
		{
			Destroy(t.gameObject);
		}
		skillButtonUIs.Clear();
		//currentSelected = null;
	}

	private void Start()
	{
		var player = BoardController.Instance.GetPlayer();
		BuildPlayerSkill(player);
	}

	public void BuildPlayerSkill(PlayerEntity player)
	{
		Clear();
		this.player = player;
		if (player == null) return;

		foreach (var ability in player.LearnedAbilities)
		{
			if (ability == null) continue;
			CreateSkillItem(ability);
		}

		player.OnAbilityLearned += OnAbilityLearned;
		player.OnAbilityCooldownChanged += OnAbilityCooldownChanged;
	}
	private void CreateSkillItem(BaseCharacterAbility ability)
	{
		if (ability.abilityType == AbilityType.Active)
		{
			var ui = Instantiate(skillButtonPrefab, skillListContainer);
			ui.SetAbility(ability);
			skillButtonUIs[ability] = ui;
		}
		else if (ability.abilityType == AbilityType.Passive)
		{
			var ui = Instantiate(passiveSkillPrefab, skillListContainer);
			ui.SetAbility(ability);
		}
	}

	private void OnAbilityLearned(BaseCharacterAbility ability)
	{
		if (ability == null) return;

		if (ability.abilityType == AbilityType.Active && skillButtonUIs.ContainsKey(ability))
			return;

		CreateSkillItem(ability);
	}

	private void OnAbilityCooldownChanged(BaseCharacterAbility ability, int cd)
	{
		if (skillButtonUIs.TryGetValue(ability, out var ui))
		{
			int max = Mathf.Max(1, ability.cooldownTurns);
			float normalized = Mathf.Clamp01(cd / (float)max);
			ui.SetCooldownVisual(normalized);
		}
	}

	private void OnEnable()
	{
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnActiveskillSelected, this);
	}

	private void OnDisable()
	{
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnActiveskillSelected, this);
	}

}
