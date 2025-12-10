using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshPro textMesh;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) Play("-1", this.transform.position);
    }

    public void Play(string value, Vector3 pos, float duration = 0.8f)
    {

        transform.position = pos;
        transform.localScale = Vector3.one * 0.7f;

        textMesh.text = value;
        textMesh.alpha = 1f;

        float sideOffset = UnityEngine.Random.Range(-0.1f, 0.1f);

        Vector3 upPos = pos + new Vector3(sideOffset, 0.25f, 0);

        Vector3 downPos = new Vector3(upPos.x, pos.y - 0.1f, pos.z);


        DOTween.Sequence()
            .Append(transform.DOScale(1.1f, 0.15f).SetEase(Ease.OutBack))
            .Append(transform.DOMove(upPos, duration * 0.6f).SetEase(Ease.OutQuad))
            .Append(transform.DOMove(downPos, duration * 0.6f).SetEase(Ease.InQuad))
            .Join(textMesh.DOFade(0f, duration))

            .OnComplete(() =>
            {
                transform.localPosition = Vector3.zero;
            });
    }




}