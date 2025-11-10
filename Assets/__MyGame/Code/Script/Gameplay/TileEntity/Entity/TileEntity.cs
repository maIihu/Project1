using System;
using System.Collections;
using System.Collections.Generic;
using __MyGame.Code.Script;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using uPools;

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

	protected bool _isDead;

	protected bool isDying;
	//events
	public event System.Action<int, int> OnHealthChanged;
	public event System.Action<int> OnArmorChanged;
	public event System.Action<TileEntity> OnDied;

	//animation stuff
	[FormerlySerializedAs("textPunch")][SerializeField] private FloatingText floatingText;
	[Header("DOTween Move")]
	[SerializeField] public float moveAnimPerTile = 0.10f;
	[SerializeField] public Ease moveEase = Ease.InOutSine;
	[SerializeField] public bool useJumpArc = true;
	[SerializeField] public float arcHeight = 0.25f;

	[Header("Squash/Stretch")]
	[SerializeField] private Transform sprite;
	[SerializeField] private float squashX = 1.08f;
	[SerializeField] private float squashY = 0.88f;
	[SerializeField] private float squashRecover = 0.06f;

	[Header("Bump")]
	[SerializeField] private float bumpDistance = 0.9f;
	[SerializeField] private float bumpDuration = 0.9f;
	[SerializeField] private Ease bumpEaseOut = Ease.OutQuad;
	[SerializeField] private Ease bumpEaseIn = Ease.InQuad;
	public virtual void OnCollision() { }

	private void OnEnable()
	{
		_isDead = false;
	}

	public void TakeDamage(int damage)
	{
		Debug.Log("Taking Damage: " + damage);
		if (_isDead) return;
		var text = Instantiate(floatingText);
		text.PunchText(transform.position, damage.ToString());
		int abosrbedByArmor = Mathf.Min(armor, damage);
		if (abosrbedByArmor > 0)
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
		if (currentHP <= 0)
		{
			Die();
			//BeginDeath();
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
	#region
	protected void BeginDeath()
	{
		if (_isDead || isDying) return;
		isDying = true;
		OnDied?.Invoke(this);
	}
	protected virtual IEnumerator DeathRoutine()
	{
		_isDead = true;
		yield return null;
		Destroy(gameObject);
	}

	public IEnumerator AnimateHop(Vector3 from, Vector3 to, float duration)
	{
		transform.position = from;

		Sequence squashSeq = null;
		if (sprite)
		{
			var baseScale = sprite.localScale;
			squashSeq = DOTween.Sequence()
				.Append(sprite.DOScale(new Vector3(baseScale.x * squashX, baseScale.y * squashY, baseScale.z), duration * 0.35f))
				.Append(sprite.DOScale(new Vector3(baseScale.x / squashX, baseScale.y / squashY, baseScale.z), duration * 0.25f))
				.AppendInterval(squashRecover)
				.Append(sprite.DOScale(baseScale, duration * 0.40f))
				.SetLink(sprite.gameObject);
		}
		Tween moveT;
		if (useJumpArc)
			moveT = transform.DOJump(to, arcHeight, 1, duration).SetEase(moveEase);
		else
			moveT = transform.DOMove(to, duration).SetEase(moveEase);

		moveT.SetLink(gameObject);

		if (squashSeq != null) squashSeq.Join(moveT);
		yield return (squashSeq != null ? squashSeq.WaitForCompletion() : moveT.WaitForCompletion());
		transform.position = to;
	}

	public IEnumerator AnimateBump(Vector3 at, Vector3 dir)
	{
		transform.position = at;
		Vector3 target = at + dir.normalized * bumpDistance;

		var baseScale = sprite ? sprite.localScale : Vector3.one;

		Sequence seq = DOTween.Sequence().SetLink(gameObject);
		if (sprite)
			seq.Append(sprite.DOScale(new Vector3(baseScale.x * squashX, baseScale.y * squashY, baseScale.z), bumpDuration * 0.5f));
		seq.Join(transform.DOMove(target, bumpDuration * 0.5f).SetEase(bumpEaseOut));
		seq.Join(sprite.DOShakePosition(bumpDuration * 0.5f, bumpDistance * 0.1f, 10, 90, false, true));
		seq.Append(transform.DOMove(at, bumpDuration * 0.5f).SetEase(bumpEaseIn));
		if (sprite)
			seq.Append(sprite.DOScale(baseScale, bumpDuration * 0.5f));

		yield return seq.WaitForCompletion();
		transform.position = at;
	}
	#endregion
}
