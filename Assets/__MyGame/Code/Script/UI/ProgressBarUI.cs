using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private RectTransform circle;
        [SerializeField] private RectTransform start;
        [SerializeField] private RectTransform end;
        [SerializeField] private TextMeshProUGUI text;

        public void UpdateProgress(float value, float maxValue)
        {
            var delta = end.localPosition - start.localPosition;
            Vector3 step = delta / maxValue;
            circle.localPosition = start.localPosition + step * value;
            text.text = $"Turn {value} / {maxValue}";
        }
    }
}