using _MyCore.DesignPattern.Singleton;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace __MyGame.Code.Script
{
    public class BoardController : Singleton<BoardController>
    {
        [SerializeField] private Node nodePrefab;
        [SerializeField] private Transform nodeContainer;
        [SerializeField] private PlayerEntity playerPrefab;
        [SerializeField] private DoorEntity doorPrefab;
        [SerializeField] private EnemyEntity enemyPrefab;

        [SerializeField] private Transform entityContainer;

        [SerializeField] private MapData[] mapDataArray;

        [SerializeField] private CharacterClass testClass;
	    [SerializeField] private EnemyType[] enemyTypeArr;
        
        private GameLogic logic;

        public static readonly int BoardSize = 6;

        private List<Node> _nodeInBoard = new List<Node>();
        private List<TileEntity> entitiesInBoard;
        public PlayerEntity player;
        public List<EnemyEntity> enemyEntities;
        public List<ObstacleEntity> obstacleEntities;

		//input block 
		private bool isAnimating;
		//public bool IsAnimating => isAnimating;
		
		//init map in scene test 
		[SerializeField] private MapType editorMapType = MapType.Green;
		[SerializeField] private bool editorSpawnSpikes = true;

		[SerializeField] private MapEffectRunner mapEffectRunner;

		public MapData CurrentMapData { get; private set; }
		
		private void Awake()
        {
            Initialize(this);
        }

        public void InitBoard()
        {
            logic = GameplayManager.Instance.GameLogic;
            _nodeInBoard = new List<Node>();
            entitiesInBoard = new List<TileEntity>();
            enemyEntities = new List<EnemyEntity>();
            obstacleEntities = new List<ObstacleEntity>();

            SpawnMapWithType(MapType.Red);
            SpawnPlayerRandomly();
        }
        
        public void ClearBoard()
        {
	        //foreach (var node in _nodeInBoard)
	        foreach (var node in _nodeInBoard.Where(node => node != null)) Destroy(node.gameObject);
	        
	        foreach (var enity in entitiesInBoard.Where(enity => enity != null)) Destroy(enity.gameObject);

	        foreach (var obstacle in obstacleEntities.Where(obstacle => obstacle)) Destroy(obstacle.gameObject);

	        foreach (var enemy in enemyEntities.Where(enemy => enemy)) Destroy(enemy.gameObject);
	        
	        _nodeInBoard.Clear();
	        entitiesInBoard.Clear();
	        enemyEntities.Clear();
	        obstacleEntities.Clear();
        }

        public void SpawnDoor()
        {            
	        //Debug.Log("Spawning door");
	        var free = _nodeInBoard.Where(n => n.OccupiedEntity == null).OrderBy(_nodeInBoard => Random.value).First();
	        var door = Instantiate(doorPrefab, free.transform.position, Quaternion.identity,  entityContainer);
	        door.InitDoor();
	        free.OccupiedEntity = door;
	        entitiesInBoard.Add(door);
        }
        
        private void SpawnMapWithType(MapType mapType)
        {
            foreach (var map in mapDataArray)
            {
                if (map.mapType == mapType)
                {
	                CurrentMapData = map;
                    GenerateBoard(map);
                }
            }
        }

        private void GenerateBoard(MapData mapData)
        {
            var offset = BoardSize / 2;
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    var positionToSpawn = new Vector3(i - offset, j - offset, 0);
                    var node = Instantiate(nodePrefab, positionToSpawn, Quaternion.identity, nodeContainer);
                    node.name = node.GridPos.ToString();
                    node.SetBaseSprite((i + j) % 2 == 0 ? mapData.sprite1 : mapData.sprite2);
                    _nodeInBoard.Add(node);
                }
            }
			mapEffectRunner.ApplyAll(mapData,this);
		}

        #region 
        private void SpawnPlayerRandomly()
        {
            var free = _nodeInBoard.Where(n => n.OccupiedEntity == null).OrderBy(n => Random.value).First();
            player = Instantiate(playerPrefab, free.transform.position, Quaternion.identity, entityContainer);
            player.CharacterInitial(testClass);
            player.RefreshUI();
            player.SyncWorldPosToGrid();
            player.OnDied += RemoveEntity;
            free.OccupiedEntity = player;
            entitiesInBoard.Add(player);
        }
        
        private void SpawnEnemiesToMap(int amount)
        {
            var freeNodes = _nodeInBoard.Where(n => n.OccupiedEntity == null).OrderBy(_nodeInBoard => Random.value).Take(amount);
            foreach (var node in freeNodes)
            {
                var enemy = Instantiate(enemyPrefab, node.transform.position, Quaternion.identity, entityContainer);
                enemy.EnemyInit(GetRandomEnemy());
                enemy.RefreshUI();
                enemy.SyncWorldPosToGrid();
                enemy.OnDied += RemoveEntity;
                bool isGhost = enemy.HasTrait<IGhostMove>();
                if (!isGhost)
                {
                    node.OccupiedEntity = enemy;
                }
                enemyEntities.Add(enemy);
                entitiesInBoard.Add(enemy);
            }
        }

        private EnemyType GetRandomEnemy()
        {
	        float spawnModifier = GameplayManager.Instance.SpawnModifier; 
	        //Debug.Log(spawnModifier);
    
	        float sumBase = enemyTypeArr.Sum(e => e.spawnWeight);

	        var adjustedWeights = enemyTypeArr.Select(e =>
	        {
		        float baseRate = e.spawnWeight / sumBase; 
		        float difficultyBias = Mathf.Lerp(1f, 1f + (e.spawnWeight - 0.5f) * 0.4f, spawnModifier);
		        float noise = Random.Range(0.9f, 1.1f);

		        return baseRate * difficultyBias * noise;
	        }).ToList();

	        float sumAdjusted = adjustedWeights.Sum();
	        var normalized = adjustedWeights.Select(w => w / sumAdjusted).ToList();

	        float r = Random.value;
	        float cumulative = 0;
	        for (int i = 0; i < enemyTypeArr.Length; i++)
	        {
		        cumulative += normalized[i];
		        if (r < cumulative)
			        return enemyTypeArr[i];
	        }

	        return enemyTypeArr.Last();
        }

        
        private void RemoveEntity(TileEntity ent)
        {
            var node = GetNodeWithEntity(ent);
            if (node != null && ReferenceEquals(node.OccupiedEntity, ent))
            {
                node.OccupiedEntity = null;
            }
            entitiesInBoard.Remove(ent);
            var asEnemy = ent as EnemyEntity;
            if (asEnemy) enemyEntities.Remove(asEnemy);
            if (ReferenceEquals(ent, player)) player = null;
        }
        public ObstacleEntity SpawnObstacleEntityAtNode(ObstacleEntity obstacle, Node spawnNode)
        {
            var obstacleEnt = Instantiate(obstacle, spawnNode.transform.position, Quaternion.identity, entityContainer);
            obstacleEnt.SyncWorldPosToGrid();
            spawnNode.OccupiedEntity = obstacleEnt;
            obstacleEnt.OnDied += RemoveEntity;
            obstacleEntities.Add(obstacleEnt);
            entitiesInBoard.Add(obstacleEnt);
            return obstacleEnt;
        }
        public PlayerEntity GetPlayer() => player;
        public List<EnemyEntity> GetEnemies() => enemyEntities;
        public List<TileEntity> GetAllEntities() => entitiesInBoard;

        public Node GetNodeWithEntity(TileEntity ent) => _nodeInBoard.FirstOrDefault(n => n.OccupiedEntity == ent);
        public Node GetNodeAtPosition(Vector2 pos) => _nodeInBoard.FirstOrDefault(n => n.GridPos == pos);
        public List<Node> AllNode => _nodeInBoard;

		#endregion
		
		public IEnumerator ShiftAnimated(Vector2 dir)
		{
			if (isAnimating) yield break;
			isAnimating = true;
			GameplayManager.Instance.LockInput();
			var ents = GetAllEntities();
			var startMap = new Dictionary<TileEntity, Vector3>(ents.Count);
			foreach (var e in ents) if (e) startMap[e] = e.transform.position;

			var logic = new GameLogic(this);
			logic.Shift(dir);

			int remaining = 0;
			foreach (var e in ents)
			{
				if (!e || !startMap.ContainsKey(e)) continue;

				var start = startMap[e];
				var end = e.transform.position;
				var moved = (Vector3)end != start;

				int steps = Mathf.RoundToInt(Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y));
				float dur = Mathf.Max(0.04f, e.moveAnimPerTile * Mathf.Max(1, steps));

				e.transform.position = start;

				remaining++;

				if (moved)
				{
					StartCoroutine(RunSlide(e, start, end, dur, () => remaining--));
				}
				else
				{
					var frontNode = GetNodeAtPosition(new Vector2(start.x,start.y) + dir);
					if (frontNode && frontNode.OccupiedEntity)
					{
						StartCoroutine(RunBump(e, start, (Vector3)dir, () => remaining--));
					}
					else
					{
						e.transform.position = end;
						remaining--;
					}
				}
			}

			yield return new WaitUntil(() => remaining <= 0);
            isAnimating = false;
			GameplayManager.Instance.OnShiftFinishedAfterMoved();
			
			SpawnEnemiesToMap(1);
		}

		// Helpers
		private IEnumerator RunSlide(TileEntity e, Vector3 from, Vector3 to, float duration, System.Action done)
		{
			yield return e.AnimateSlide(from, to, duration);
			e.transform.position = to;
			e.SyncWorldPosToGrid();
			done?.Invoke();
		}

		private IEnumerator RunBump(TileEntity e, Vector3 at, Vector3 dir, System.Action done)
		{
			yield return e.AnimateBump(at, dir); 
			e.transform.position = at;
			done?.Invoke();
		}
		#region ----------Editor----------


		//[ContextMenu("Preview/Clear Board In Editor")]
		//private void Editor_ClearBoard()
		//{
		//	if (nodeContainer)
		//	{
		//		for (int i = nodeContainer.childCount - 1; i >= 0; i--)
		//			DestroyImmediate(nodeContainer.GetChild(i).gameObject);
		//	}
		//	if (entityContainer)
		//	{
		//		for (int i = entityContainer.childCount - 1; i >= 0; i--)
		//			DestroyImmediate(entityContainer.GetChild(i).gameObject);
		//	}

		//	_nodeInBoard = new List<Node>();
		//	entitiesInBoard = new List<TileEntity>();
		//	enemyEntities = new List<EnemyEntity>();
		//	obstacleEntities = new List<ObstacleEntity>();
		//}

		#endregion


	}
}
