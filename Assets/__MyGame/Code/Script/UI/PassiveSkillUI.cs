using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillUI : MonoBehaviour
{
	[SerializeField] private Image skillIcon;
	[SerializeField] private Image cooldownOverlay;
	private BaseCharacterAbility ability;
	public void SetAbility(BaseCharacterAbility ability)
	{
		this.ability = ability;
		skillIcon.sprite = ability.abilityIcon;
	}
	public void SetCooldownVisual(float normalized01)
	{
		if (cooldownOverlay) cooldownOverlay.fillAmount = Mathf.Clamp01(normalized01);
	}
}
