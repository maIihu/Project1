using _MyCore.DesignPattern.Observer.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillButtonUI : MonoBehaviour
{
	[SerializeField] private Image skillIcon;
	[SerializeField] private Image cooldownOverlay;
	[SerializeField] private Button btn;
	private BaseCharacterAbility ability;

	private void Awake()
	{
		if (cooldownOverlay) cooldownOverlay.fillAmount = 0f;
		btn.onClick.AddListener(onClick);
	}
	public void SetAbility(BaseCharacterAbility ability)
	{
		this.ability = ability;
		skillIcon.sprite = ability.abilityIcon;
	}
	public void SetCooldownVisual(float normalized01)
	{
		if(cooldownOverlay) cooldownOverlay.fillAmount = Mathf.Clamp01(normalized01);
	}
	private void onClick()
	{
		if (ability != null)
		{
			MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnActiveskillSelected));
		}
	}
}
