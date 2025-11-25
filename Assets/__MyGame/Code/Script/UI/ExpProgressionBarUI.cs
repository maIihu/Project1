using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpProgressionBarUI : MonoBehaviour
{
	[SerializeField] private RectTransform circle;
	[SerializeField] private RectTransform start;
	[SerializeField] private RectTransform end;
	[SerializeField] private TextMeshProUGUI expText;
	[SerializeField] private TextMeshProUGUI levelText;

	public void UpdateProgress(float value, float maxValue)
	{
		var delta = end.localPosition - start.localPosition;
		Vector3 step = delta / maxValue;
		circle.localPosition = start.localPosition + step * value;
	}

	public void UpdateExp(string text)
	{
		expText.text = text;
	}
	public void UpdateLevel(string text)
	{
		levelText.text = text;
	}
}
