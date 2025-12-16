using System;
using System.Collections.Generic;
using __MyGame.Code.Script.Helper;
using _MyCore.DesignPattern.Observer.Runtime;
using _MyCore.DesignPattern.Singleton;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __MyGame.Code.Script
{
    public enum GameState
    {
        Playing, Pause, 
    }
    
    public class GameplayManager : Singleton<GameplayManager>, IMessageHandle
    {
        [SerializeField] private BoxCollider2D boundCol;

        [SerializeField] public BoardController board;

        public SkillSelectedUIController skillSelectedUIController;

		public AbilityPipeLine abilityPipeLine = new AbilityPipeLine();

        private float _progressFactor; // step control
        private float _playerFactor; // level player
        private float _mapDifficulty; 
        private float _randomFluctuation;
        private float _mapLevel;
        private int _stepMoveCounter;
        private int _maxStepMoveLevel;
        private int i = 1;
        private int _inputLockCount;
        private GameState _currentState;
        
        public bool IsInputLocked => _inputLockCount > 0;
        private List<Func<UniTask>> postMoveActions = new List<Func<UniTask>>();
		public GameLogic GameLogic { get; private set; }
        
        public float SpawnModifier { get; private set; }

        #region ----------Event Func----------

        private void Awake()
        {
            Initialize(this);
        }

        private void OnEnable()
        {
            MessageManager.Instance.AddSubscriber(ProjectMessageType.OnShowPopup, this);
        }

        private void OnDisable()
        {
            MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnShowPopup, this);
        }

        private void Start()
        {
            InitGame();
        }

        private void InitGame()
        {
            GameLogic = new GameLogic(this.board);
            CameraFit();
            board.InitBoard();
            
            _mapLevel = 1f;
            _mapDifficulty = board.CurrentMapData.mapDifficulty;
            _playerFactor = 1f;
            _stepMoveCounter = 0;
            _maxStepMoveLevel = board.CurrentMapData.stepsToNextLevel;
            CalculateSpawnRate();

            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnLoadGame));
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnMoveControl, 
                new object[]{_stepMoveCounter, _maxStepMoveLevel}));
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnGameStart));
            skillSelectedUIController.InitiateReference();
            
            ChangeState(GameState.Playing);
            
            AudioManager.Instance.StopMusic("BG_Menu");
            AudioManager.Instance.PlayMusic("BG_Game", 0.8f, true);
        }

        public GameState CurrentState() => _currentState;
        
        public void ChangeState(GameState state)
        {
            _currentState = state;
            switch (_currentState)
            {
                case GameState.Playing:
                    break;
                case GameState.Pause:
                    break;
            }
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     LoadNewMapLevel();
            // }
        }

        public void ReloadAllLevel()
        {
            BoardController.Instance.ClearBoard();
            board.LoadNewMapWithPlayer();
            _stepMoveCounter = 0;
            _maxStepMoveLevel = board.CurrentMapData.stepsToNextLevel;
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnGameReload,
                new object[]{_stepMoveCounter, _maxStepMoveLevel}));
            ChangeState(GameState.Playing);
            Debug.Log("Reload");
        }
        
        public void RegisterPostMoveAction(Func<UniTask> action)
		{
            if(action != null)
			    postMoveActions.Add(action);
		}

        public async void OnShiftFinishedAfterMoved()
        {
            if (postMoveActions.Count == 0)
            {
                UnlockInput();
                return;
            }
            var actions = postMoveActions.ToArray();
            postMoveActions.Clear();

            foreach (var action in actions)
            {
                try
                {
                    await action();
                }
                catch(Exception e)
                {
                    Debug.Log($"Error when executing post-move action: {e}");
				}
            }
            UnlockInput();
		}
		public void LockInput()
        {
            _inputLockCount++;
        }
        public void UnlockInput()
        {
			_inputLockCount = Mathf.Max(0, _inputLockCount - 1);
		}

        private void CameraFit()
        {
            boundCol.enabled = true;
            var bound = boundCol.bounds;
            CameraManager.Instance.FitCameraToBounds(bound);
            boundCol.enabled = false;
        }

        public void OnEntityDamaged(TileEntity damagedEntity,TileEntity attacker, int damageTaken)
        {
            if(damagedEntity is PlayerEntity player)
            {
                foreach(var ability in player.LearnedAbilities)
                {
                    if (ability == null) continue;
                    if (ability.phase != CastPhase.Reaction) continue;
                    if (!player.CanUse(ability)) continue;

                    var ctx = new AbilityContext
                    {
                        board = board,
                        gameLogic = GameLogic,
                        targetNode = attacker != null ? board.GetNodeWithEntity(attacker) : null,
                        direction = attacker != null ? Vector2Int.RoundToInt((Vector2)(attacker.transform.position - player.transform.position))
                    : Vector2Int.zero
                    };
					if (!ability.CanCast(player, ctx)) continue;
                    ability.OnCast(player, ctx);
                    player.StartCooldown(ability);

				}
			}
            if(damagedEntity is EnemyEntity enemy)
            {
                var node = board.GetNodeWithEntity(enemy);
                foreach (var t in enemy.enemyType.enemyTraits)
                {
                    if(t is IOnDamaged reactive)
                    {
                        reactive.OnDamaged(board, enemy, attacker, damageTaken);
					}
                }
			}
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
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnMoveControl, 
                new object[]{_stepMoveCounter, _maxStepMoveLevel}));
            
            if(_stepMoveCounter == _maxStepMoveLevel)
            {
                //Debug.Log("Step move");
                BoardController.Instance.SpawnDoor();
                return;
            }
            
            if (_stepMoveCounter == 10 * i)
            {
                i++;
                _progressFactor = 1.0f * _stepMoveCounter / board.CurrentMapData.stepsToNextLevel;
                CalculateSpawnRate();
            }
        }

        public void CalculateSpawnRate()
        {
            _randomFluctuation = Random.Range(-0.2f, 0.2f);
            SpawnModifier = _progressFactor * 0.4f + _playerFactor * 0.2f 
                                                  + _mapDifficulty * 0.3f + _randomFluctuation * 0.1f;
            SpawnModifier = Mathf.Clamp01(SpawnModifier);
        }

        #endregion

        protected override void OnRegistration()
        {
            base.OnRegistration();
            //Debug.Log("-----GameplayManager registered");
        }

        public void Handle(Message message)
        {
            switch (message.Type)
            {
                case ProjectMessageType.OnShowPopup:
                    ChangeState(GameState.Pause);
                    Debug.Log("Show popup");
                    break;
            }
        }
    }
}