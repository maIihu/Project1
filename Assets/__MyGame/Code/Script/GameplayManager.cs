using System;
using _MyCore.DesignPattern.Observer.Runtime;
using _MyCore.DesignPattern.Singleton;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [SerializeField] public BoardController board;

        [SerializeField] public GameObjectPool objectPool;

        public AbilityPipeLine abilityPipeLine = new AbilityPipeLine();

		public GameLogic GameLogic { get; private set; }

        private void Awake()
        {
            Initialize(this);
        }

        public void QueueAbility(PlayerEntity user, BaseCharacterAbility ability, AbilityContext ctx)
        {
            abilityPipeLine.Queue(new QueuedCast {
				user = user,
				ability = ability,
				context = ctx
			});
		}

        private void Start()
        {
            GameLogic = new GameLogic(this.board);
            objectPool.InitEnemyPooling();
            board.InitBoard();
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnGameStart));
        }
        

        protected override void OnRegistration()
        {
            base.OnRegistration();
            //Debug.Log("-----GameplayManager registered");
        }
    }
}