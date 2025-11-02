using System;
using System.Collections;
using System.Collections.Generic;
using __MyGame.Code.Script;
using UnityEngine;
using UnityEngine.Serialization;
using uPools;

public abstract class TileEntity : MonoBehaviour
{
	[FormerlySerializedAs("textPunch")] [SerializeField] private FloatingText floatingText;
	public Vector2Int Pos;
	public bool BlocksMovement;
	public int moveStep;

	public int maxHP;
	public int currentHP;
	public int armor;
	public int attack;

	public Sprite entitySprite;

	private bool _isDead;

	public event System.Action<int, int> OnHealthChanged;
	public event System.Action<int> OnArmorChanged;
	public event System.Action<TileEntity> OnDied;
	public virtual void OnCollision() { }

	private void OnEnable()
	{
		_isDead = false;
	}

	public void TakeDamage(int damage)
	{
		Debug.Log("Taking Damage: " + damage);
		if(_isDead) return;
		var text = Instantiate(floatingText);
		text.PunchText(transform.position, damage.ToString());
		int abosrbedByArmor = Mathf.Min(armor, damage);
		if(abosrbedByArmor > 0)
		{
			armor -= abosrbedByArmor;
			OnArmorChanged?.Invoke(armor);
		}
		int remainingDamage = damage - abosrbedByArmor;
		if (remainingDamage > 0)
		{
			currentHP = Mathf.Max(currentHP - remainingDamage, 0);
			OnHealthChanged?.Invoke(currentHP, maxHP);
		}
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
		if (_isDead) return;
		_isDead = true;
		OnDied?.Invoke(this);
		//Debug.Log("On Dead");
		//SharedGameObjectPool.Return(gameObject);
		
		Destroy(gameObject);
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
