using System;
using _MyCore.DesignPattern.Singleton;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [SerializeField] public BoardController board;

        [SerializeField] public GameObjectPool objectPool;
        
        public GameLogic GameLogic { get; private set; }

        private void Awake()
        {
            Initialize(this);
        }

        private void Start()
        {
            GameLogic = new GameLogic(this.board);
            objectPool.InitEnemyPooling();
            board.InitBoard();
        }
        

        protected override void OnRegistration()
        {
            base.OnRegistration();
            //Debug.Log("-----GameplayManager registered");
        }
    }
}