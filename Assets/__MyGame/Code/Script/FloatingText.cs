using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;

        public void PunchText(Vector3 pos, string value)
        {
            transform.position = pos;
            text.text = value;

            transform.localScale = Vector3.zero;
            text.alpha = 1f;

            Vector3 topPos = pos + new Vector3(0, 0.25f, 0);   
            Vector3 endPos = pos + new Vector3(0, -0.15f, 0);  

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOScale(Vector3.one * 1.1f, 0.25f).SetEase(Ease.OutBack));

            seq.Append(transform.DOMove(topPos, 0.35f).SetEase(Ease.OutQuad));
            seq.Append(transform.DOMove(endPos, 0.45f).SetEase(Ease.InQuad));

            seq.Append(text.DOFade(0f, 0.3f));
            seq.Join(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));

            seq.OnComplete(() => Destroy(gameObject));
        }
    }
}