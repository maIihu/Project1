using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityViewUI : MonoBehaviour
{
	[SerializeField] SpriteRenderer spriteRenderer;
	[Header("UI")]
	[SerializeField] TextMeshProUGUI hpText;
	[SerializeField] TextMeshProUGUI armorText;

	TileEntity entity;

	private void Awake()
	{
		entity = GetComponent<TileEntity>();
		entity.OnHealthChanged += HandleHP;
		entity.OnArmorChanged += HandleArmor;
		entity.OnSpriteChanged += SetSprite;
		HandleHP(entity.currentHP,entity.maxHP);
		HandleArmor(entity.armor);

	}

	private void OnDestroy()
	{
		entity.OnHealthChanged -= HandleHP;
		entity.OnArmorChanged -= HandleArmor;
		entity.OnSpriteChanged -= SetSprite;
	}
	private void HandleHP(int cur, int max)
	{
		hpText.text = $"{cur}";
	}
	private void HandleArmor(int armor)
	{
		armorText.text = armor.ToString();
	}
	public void SetSprite(Sprite s)
	{
		spriteRenderer.sprite = s;
	}
}
