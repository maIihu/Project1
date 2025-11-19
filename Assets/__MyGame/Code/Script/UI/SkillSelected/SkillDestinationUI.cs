using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillDestinationUI : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private TextMeshProUGUI notificationText;

	private void Awake()
	{
		canvasGroup.alpha = 0f;
		notificationText.text = "";
	}
	public void Show(string text)
	{
		notificationText.text = text;
		canvasGroup.alpha = 1f;
	}

	public void Hide()
	{
		canvasGroup.alpha = 0f;
		notificationText.text = "";
	}
}
