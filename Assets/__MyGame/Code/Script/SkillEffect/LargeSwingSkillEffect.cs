using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LargeSwingSkillEffect : BaseSkillEffect
{

	public void Play(Vector2Int dir)
	{
		token?.Cancel();
		token?.Dispose();
		token = new CancellationTokenSource();
		_ = PlayAnimation(dir, token.Token);
	}
	public override async UniTask PlayAnimation(Vector2Int dir, CancellationToken cts)
	{
		if(dir == Vector2.left)
		{
			spriteRender.sprite = frames[3];
			await UniTask.Delay((int)(frameRate * 1000), cancellationToken: cts);
		}
		else if(dir == Vector2.up)
		{
			spriteRender.sprite = frames[1];
			await UniTask.Delay((int)(frameRate * 1000), cancellationToken: cts);
		}
		else if(dir == Vector2.down)
		{
			spriteRender.sprite = frames[0];
			await UniTask.Delay((int)(frameRate * 1000), cancellationToken: cts);
		}
		else
		{
			spriteRender.sprite = frames[2];
			await UniTask.Delay((int)(frameRate * 1000), cancellationToken: cts);
		}
		Destroy(gameObject);
	}
}
