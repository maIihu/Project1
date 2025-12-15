using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BaseSkillEffect : MonoBehaviour
{
	[SerializeField] protected SpriteRenderer spriteRender;
	[SerializeField] protected Sprite[] frames;
	[SerializeField] protected float frameRate = 0.1f;

	protected CancellationTokenSource token;

	private void Awake()
	{
		
	}

	private void OnDisable()
	{
		token?.Cancel();
		token?.Dispose();	
		token = null;
	}

	public void Play()
	{
		token?.Cancel();
		token?.Dispose();
		token = new CancellationTokenSource();
		_ = PlayAnimation(Vector2Int.zero, token.Token);
	}

	public virtual async UniTask PlayAnimation(Vector2Int dir, CancellationToken cts)
	{
		float angle = 0f;
		if (dir == Vector2Int.right) angle = -90f;
		else if(dir == Vector2Int.up) angle = -180f;
		else if (dir == Vector2Int.left) angle = 180f;
		else if (dir == Vector2Int.down) angle = 0f;
		else angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, angle);

		for (int i = 0; i < frames.Length; i++) 
		{
			if(cts.IsCancellationRequested) return;
			spriteRender.sprite = frames[i];
			await UniTask.Delay((int)(frameRate * 1000), cancellationToken: cts);
		}
		Destroy(gameObject);
	}
}
