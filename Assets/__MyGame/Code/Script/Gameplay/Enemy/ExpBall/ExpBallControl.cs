using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __MyGame.Code.Script.Gameplay.Enemy.ExpBall
{
    public class ExpBallControl : MonoBehaviour
    {
        [SerializeField] private GameObject expballPrefab;
        [SerializeField] private int amount = 5;
        
        private List<GameObject> expBallHolder;
        
        public void Init()
        {
            expBallHolder = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                var go = Instantiate(expballPrefab, transform);
                expBallHolder.Add(go);
                go.SetActive(false);
            }
        }
        
        public void Play(Vector3 sourcePos)
        {
            for (int i = 0; i < expBallHolder.Count; i++)
            {
                var ball = expBallHolder[i];
                ball.SetActive(true);

                Vector3 origin = sourcePos;
                ball.transform.position = origin;

                Vector3 offset = new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-.8f, 0.8f), 0);
                Vector3 burstPos = origin + offset;
                
                Sequence seq = DOTween.Sequence().SetTarget(this);
                
                seq.Append(ball.transform.DOMove(burstPos, 0.3f).SetEase(Ease.OutSine))
                    .AppendInterval(0.05f)
                    .Append(ball.transform.DOMove(this.transform.position, 0.5f).SetEase(Ease.InSine))
                    .OnComplete(() => { ball.SetActive(false); });
            }
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}