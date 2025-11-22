using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UIFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float duration;

    public void Fade()
    {
        fadeGroup.alpha = 1;
        fadeGroup.DOFade(0f, duration).SetUpdate(true).SetEase(Ease.InSine);

    }
    
}