using System;
using _MyCore.DesignPattern.Singleton;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [SerializeField] public BoardController board;

        private void Awake()
        {
            Initialize(this);
        }

        protected override void OnRegistration()
        {
            base.OnRegistration();
            Debug.Log("-----GameplayManager registered");
        }
    }
}