using __MyGame.Code.Script.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthUIManager : MonoBehaviour
{
	[SerializeField] private ExpProgressionBarUI expBar;

	private PlayerEntity player;
	public void Initial(PlayerEntity playerEntity)
	{
		player = playerEntity;
		player.OnExpChanged += HandleExpChanged;
		player.OnLevelChanged += HandleLevelChanged;

		HandleLevelChanged(player.level);
		HandleExpChanged(player.currentExp, player.ExpToNextLevel);

	}
	private void OnDestroy()
	{
		if (player != null)
		{
			player.OnExpChanged -= HandleExpChanged;
			player.OnLevelChanged -= HandleLevelChanged;
		}
		player = null;
	}

	private void OnDisable()
	{
		player.OnLevelChanged -= HandleLevelChanged;
		player.OnExpChanged -= HandleExpChanged;
		player = null;
	}
	private void HandleLevelChanged(int level)
	{
		expBar.UpdateLevel($"Level {level}");
	}
	private void HandleExpChanged(int currentExp, int expToNextLevel)
	{
		expBar.UpdateProgress(currentExp, expToNextLevel);
		expBar.UpdateExp($"{currentExp} / {expToNextLevel} EXP");
	}
}
