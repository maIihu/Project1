using System;
using __MyGame.Code.Script.Helper;
using _MyCore.DesignPattern.Observer.Runtime;
using _MyCore.DesignPattern.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __MyGame.Code.Script
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [SerializeField] private BoxCollider2D boundCol;

        [SerializeField] public BoardController board;

        [SerializeField] public GameObjectPool objectPool;

        public AbilityPipeLine abilityPipeLine = new AbilityPipeLine();

        private float _progressFactor;
        private float _playerFactor;
        private float _mapDifficulty;
        private float _randomFluctuation;
        
        private int _stepMoveCounter;

        public GameLogic GameLogic { get; private set; }
        
        public float SpawnModifier { get; private set; }

        #region ----------Event Func----------

        private void Awake()
        {
            Initialize(this);
        }

        private void Start()
        {
            CameraFit();
            GameLogic = new GameLogic(this.board);
            objectPool.InitEnemyPooling();
            board.InitBoard();
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnGameStart));
        }

        private void CameraFit()
        {
            boundCol.enabled = true;
            var bound = boundCol.bounds;
            CameraManager.Instance.FitCameraToBounds(bound);
            boundCol.enabled = false;
        }

        #endregion

        #region ----------Public Method----------

        public void QueueAbility(PlayerEntity user, BaseCharacterAbility ability, AbilityContext ctx)
        {
            abilityPipeLine.Queue(new QueuedCast
            {
                user = user,
                ability = ability,
                context = ctx
            });
        }

        public void StepMoveCount()
        {
            _stepMoveCounter++;
            // thay doi moi 10 lan
            if (_stepMoveCounter == 10)
            {
                //TODO: tinh lai progressFactor
                CalculateSpawnRate();
            }
        }

        public void CalculateSpawnRate()
        {
            _randomFluctuation = Random.Range(-0.2f, 0.2f);
            SpawnModifier = _progressFactor * 0.4f + _playerFactor * 0.2f 
                                                  + _mapDifficulty * 0.3f + _randomFluctuation * 0.1f;
        }

        #endregion

        protected override void OnRegistration()
        {
            base.OnRegistration();
            //Debug.Log("-----GameplayManager registered");
        }
    }
}