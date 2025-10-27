using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEntity : MonoBehaviour
{
	public Vector2Int Pos;
	public bool BlocksMovement;
	public int moveStep;

	public int maxHP;
	public int currentHP;
	public int armor;
	public int attack;

	public Sprite entitySprite;

	public event System.Action<int, int> OnHealthChanged;
	public event System.Action<int> OnArmorChanged;
	public virtual void OnCollision() { }
	public void TakeDamage(int damage)
	{
		int remain = Mathf.Max(damage - armor, 0);
		currentHP = Mathf.Max(currentHP - remain, 0);
		OnHealthChanged?.Invoke(currentHP, maxHP);
		if(currentHP <= 0)
		{
			Die();
		}
	}

	public void SetArmor(int value)
	{
		armor = Mathf.Max(value, 0);
		OnArmorChanged?.Invoke(armor);

	}
	public void Die()
	{
		//die logic
	}
	public void SyncWorldPosToGrid()
	{
		Pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
	}
	public void RefreshUI()
	{
		OnHealthChanged?.Invoke(currentHP, maxHP);
		OnArmorChanged?.Invoke(armor);
	}
	public event System.Action<Sprite> OnSpriteChanged;

	public void SetSpriteRuntime(Sprite s)  
	{
		entitySprite = s;
		OnSpriteChanged?.Invoke(entitySprite);
	}

}
