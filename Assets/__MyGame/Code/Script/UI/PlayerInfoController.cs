using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
	[SerializeField] private Image characterClassPortait;
	[SerializeField] private TextMeshProUGUI playerHealth;
	[SerializeField] private TextMeshProUGUI playerAttack;
	[SerializeField] private TextMeshProUGUI playerArmor;

	private PlayerEntity player;

	public void Bind(PlayerEntity player)
	{
		Unbind();
		this.player = player;
		if (player == null) return;
		characterClassPortait.sprite = player.entityPortrait;
		playerAttack.text = $"Attack: {player.attack.ToString()}";
		playerArmor.text = $"Armor: {player.armor.ToString()}";
		playerHealth.text = $"Hp: {player.currentHP.ToString()}/{player.maxHP.ToString()}";

		player.OnHealthChanged += OnHealthChanged;
		player.OnArmorChanged += OnArmorChanged;
		player.OnSpriteChanged += OnSpriteChanged;

		player.RefreshUI();
	}
	public void Unbind()
	{
		if(player != null)
		{
			player.OnHealthChanged -= OnHealthChanged;
			player.OnArmorChanged -= OnArmorChanged;
			player.OnSpriteChanged -= OnSpriteChanged;
		}
		player = null;
	}
	private void OnHealthChanged(int current,int max)
	{
		playerHealth.text = $"Hp: {current.ToString()}/{max.ToString()}";
	}
	private void OnArmorChanged(int armor)
	{
		playerArmor.text = $"Armor: {armor.ToString()}";
	}
	private void OnSpriteChanged(Sprite s)
	{
		characterClassPortait.sprite = s;
	}
}