using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityStatView : MonoBehaviour
{
	[SerializeField] SpriteRenderer spriteRenderer;
	[Header("UI")]
	[SerializeField] TextMeshPro hpText;
	[SerializeField] TextMeshPro armorText;
	[SerializeField] TextMeshPro attackText;
	TileEntity entity;

	private void OnEnable()
	{
		entity = GetComponent<TileEntity>();
		entity.OnHealthChanged += HandleHP;
		entity.OnArmorChanged += HandleArmor;
		entity.OnSpriteChanged += SetSprite;

	}

	public void InitView()
	{
		HandleHP(entity.currentHP,entity.maxHP);
		HandleArmor(entity.armor);
		HandleAttack(entity.attack);
	}

	private void OnDisable()
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
	private void HandleAttack(int attack)
	{
		attackText.text = attack.ToString();
	}
}
