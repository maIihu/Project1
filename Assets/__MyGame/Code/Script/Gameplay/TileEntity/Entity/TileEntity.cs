using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using __MyGame.Code.Script;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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
	public Sprite entityPortrait;

	protected bool _isDead;

	protected bool isDying;
	//events
	public event System.Action<int, int> OnHealthChanged;
	public event System.Action<int> OnArmorChanged;
	public event System.Action<TileEntity> OnDied;

	//animation stuff
	[SerializeField] protected EntityStatView statView;
	
	[Header("DOTween Move")]
	[SerializeField] public float moveAnimPerTile = 0.10f;
	[SerializeField] public Ease moveEase = Ease.InOutSine;
	[SerializeField] public bool useJumpArc = true;
	[SerializeField] public float arcHeight = 0.25f;

	[Header("Squash/Stretch")]
	[SerializeField] public Transform sprite;
	[SerializeField] private float squashX = 1.08f;
	[SerializeField] private float squashY = 0.88f;
	[SerializeField] private float squashRecover = 0.06f;

	[Header("Bump")]
	[SerializeField] private float bumpDistance = 1f;
	[SerializeField] private float bumpDuration = 0.5f;
	[SerializeField] private Ease bumpEaseOut = Ease.OutQuad;
	[SerializeField] private Ease bumpEaseIn = Ease.InQuad;

	protected TileEntity lastAttacker;
	public TileEntity LastAttack => lastAttacker;
	public virtual void OnCollision() { }

	private void OnEnable()
	{
		_isDead = false;
	}

	public virtual void TakeDamage(int damage, TileEntity attacker = null)
	{
		lastAttacker = attacker;
		if (this is DoorEntity door)
		{
			//Debug.Log("Cant attack door");
			return;
		}
		//Debug.Log("Taking Damage: " + damage);
		if (_isDead) return;

		// var text = Instantiate(floatingText);
		// text.PunchText(transform.position, damage.ToString());
		AudioManager.Instance.PlayOneShot("Attack", 0.5f);
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
			GameplayManager.Instance.OnEntityDamaged(this, attacker, damage);
		}
		if (currentHP <= 0)
		{
			Die();
		}
	}

	public void SetArmor(int value)
	{
		armor = Mathf.Max(value, 0);
		OnArmorChanged?.Invoke(armor);
	}
	
	public virtual void Die()
	{
		if (_isDead) return;
		_isDead = true;
		OnDied?.Invoke(this);
		//Debug.Log("On Dead");

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
	protected virtual IEnumerator DeathRoutine()
	{
		_isDead = true;
		yield return null;
		Destroy(gameObject);
	}

	private void OnDisable()
	{
		DOTween.Kill(gameObject, complete: false);
		if (sprite) DOTween.Kill(sprite.gameObject, complete: false);
	}

	private void OnDestroy()
	{
		DG.Tweening.DOTween.Kill(gameObject, complete: false);
		if (sprite) DG.Tweening.DOTween.Kill(sprite.gameObject, complete: false);
	}

	public IEnumerator AnimateSlide(Vector3 from, Vector3 to, float duration)
	{
		transform.position = from;

		var seq = DOTween.Sequence().SetLink(gameObject).SetTarget(this);

		seq.Append(transform.DOMove(to, duration).SetEase(Ease.OutCubic));

		if (sprite)
		{
			var baseScale = sprite.localScale;

			seq.Join(sprite.DOScale(new Vector3(baseScale.x * squashX, baseScale.y * squashY, baseScale.z),
					duration * 0.25f).SetEase(Ease.OutQuad))
				.Append(sprite.DOScale(baseScale, duration * 0.2f).SetEase(Ease.OutQuad));
		}

		yield return seq.WaitForCompletion();
		if (_isDead) yield break;
	}


	public async UniTask AnimateHit()
	{
		await transform.DOShakePosition(0.5f,0.4f,10,90,false,true).SetLink(gameObject).AsyncWaitForCompletion();
	}

	public async UniTask AnimateJump(Vector3 from, Vector3 to, float duration = 0.4f)
	{
		transform.position = from;
		await transform.DOJump(to, arcHeight, 1, duration).SetEase(moveEase).SetLink(gameObject).AsyncWaitForCompletion();
	}
	public IEnumerator AnimateBump(Vector3 at, Vector3 dir)
	{
		transform.position = at;
		bool vertical = Mathf.Abs(dir.y) > Mathf.Abs(dir.x);
		float sgn = vertical ? Mathf.Sign(dir.y) : Mathf.Sign(dir.x);
		float half = bumpDuration * 0.5f;
		var baseScale = sprite ? sprite.localScale : Vector3.one;
		var squashA = vertical ? new Vector3(baseScale.x * squashX, baseScale.y * squashY, baseScale.z)
		: new Vector3(baseScale.x * squashY, baseScale.y * squashX, baseScale.z);


		Sequence seq = DOTween.Sequence().SetLink(gameObject);
		if (sprite)
			seq.Append(sprite.DOScale(squashA, half * 0.8f));

		if (vertical)
		{

			float toY = at.y + sgn * bumpDistance;
			seq.Join(transform.DOMoveY(toY, half).SetEase(bumpEaseOut));
			if (sprite) seq.Join(sprite.DOShakePosition(half, new Vector3(0f, bumpDistance * 0.3f, 0f), 10, 90, false, true));
			seq.Append(transform.DOMoveY(at.y, half).SetEase(bumpEaseIn));
		}
		else
		{
			float toX = at.x + sgn * bumpDistance;
			seq.Join(transform.DOMoveX(toX, half).SetEase(bumpEaseOut));
			if (sprite) seq.Join(sprite.DOShakePosition(half, new Vector3(bumpDistance * 0.3f, 0f, 0f), 10, 90, false, true));
			seq.Append(transform.DOMoveX(at.x, half).SetEase(bumpEaseIn));
		}

		if (sprite)
			seq.Append(sprite.DOScale(baseScale, half * 0.8f));

		yield return seq.WaitForCompletion();
		if (_isDead) yield break;
		transform.position = at;
	}
	#endregion
	
}
