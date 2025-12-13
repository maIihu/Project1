using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshPro textMesh;
    private Sequence seq;

    public void Play(string value, Vector3 pos, float duration = 0.8f)
    {
        seq?.Kill();
        seq = null;

        transform.position = pos;
        transform.localScale = Vector3.one * 0.7f;

        textMesh.text = value;
        textMesh.alpha = 1f;

        float sideOffset = UnityEngine.Random.Range(-0.1f, 0.1f);
        Vector3 upPos = pos + new Vector3(sideOffset, 0.25f, 0);
        Vector3 downPos = new Vector3(upPos.x, pos.y - 0.1f, pos.z);

        seq = DOTween.Sequence().SetTarget(this).SetLink(gameObject)             
            .Append(transform.DOScale(1.1f, 0.15f).SetEase(Ease.OutBack))
            .Append(transform.DOMove(upPos, duration * 0.6f).SetEase(Ease.OutQuad))
            .Append(transform.DOMove(downPos, duration * 0.6f).SetEase(Ease.InQuad))
            .Join(textMesh.DOFade(0f, duration))
            .OnComplete(() =>
            {
                FloatingTextPool.Instance.Release(this);
            });
    }

    private void OnDisable()
    {
        seq?.Kill();
        seq = null;
    }
}