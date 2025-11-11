using __MyGame.Code.Script;
using _MyCore.DesignPattern.Observer.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListController : MonoBehaviour, IMessageHandle
{
	[SerializeField] private ActiveSkillButtonUI skillButtonPrefab;
	[SerializeField] private Transform skillListContainer;

	private readonly Dictionary<BaseCharacterAbility, ActiveSkillButtonUI> skillButtonUIs = new Dictionary<BaseCharacterAbility, ActiveSkillButtonUI>();
	//private ActiveSkillButtonUI currentSelected;

	public void Handle(Message message)
	{
		switch(message.Type)
		{
			case ProjectMessageType.OnActiveskillSelected:

				break;
			case ProjectMessageType.OnActivesSkillCancled:

				break;
		}
	}

	public void BuildForm(PlayerEntity player)
	{
		Clear();
		if (player == null || player.characterClass == null) return;
		foreach(var ability in player.characterClass.abilities)
		{
			var ui = Instantiate(skillButtonPrefab, skillListContainer);
			ui.SetAbility(ability);
			skillButtonUIs[ability] = ui;
		}
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
		if (player == null) return;
		foreach (var ability in player.abilities)
		{
			var skillButton = Instantiate(skillButtonPrefab, skillListContainer);
			skillButton.SetAbility(ability.Key);
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
