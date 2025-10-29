using _MyCore.DesignPattern.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;

namespace __MyGame.Code.Script
{
    public class BoardController : Singleton<BoardController>
	{
        [SerializeField] private Node nodePrefab;
        [SerializeField] private Transform nodeContainer;
        [SerializeField] private PlayerEntity playerPrefab;
        [SerializeField] private EnemyEntity enemyPrefab;


		[SerializeField] private Transform entityContainer;
        
        [SerializeField] private MapData[] mapDataArray;

        //test 
        [SerializeField] private CharacterClass testClass;
        [SerializeField] private EmemyType testEnemyType;

		public static readonly int BoardSize = 6;

        private List<Node> _nodeInBoard;
        private List<TileEntity> entitiesInBoard;
        public PlayerEntity player;
        public List<EnemyEntity> enemyEntities; 

        private void Start()
        {
            _nodeInBoard = new List<Node>();
            entitiesInBoard = new List<TileEntity>();
			enemyEntities = new List<EnemyEntity>();

			SpawnMapWithType(MapType.Green);
			SpawmPlayerRandomly();
            SpawnEnemiesToMap(1);
        }

		private void Awake()
		{
			Initialize(this);
		}

		private void SpawnMapWithType(MapType mapType)
        {
            foreach (var map in mapDataArray)
            {
                if (map.mapType == mapType)
                {
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
                    node.SetSpriteNode((i + j) % 2 == 0 ? mapData.sprite1 : mapData.sprite2);
                    _nodeInBoard.Add(node);
                }
            }
        }

		#region
        private void SpawmPlayerRandomly()
        {
            var free = _nodeInBoard.Where(n => n.OccupiedEntity == null).OrderBy(_nodeInBoard => Random.value).First();
            player = Instantiate(playerPrefab,free.transform.position, Quaternion.identity, entityContainer);
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
            foreach(var node in freeNodes)
            {
                var e = Instantiate(enemyPrefab, node.transform.position, Quaternion.identity, entityContainer);
				e.EmemyInit(testEnemyType);
				e.RefreshUI();
				e.SyncWorldPosToGrid();
                e.OnDied += RemoveEntity;
				node.OccupiedEntity = e;
				enemyEntities.Add(e);
				entitiesInBoard.Add(e);
			}
		}
        private void RemoveEntity(TileEntity ent)
        {
            var node = GetNodeWithEntity(ent);
            if(node != null && ReferenceEquals(node.OccupiedEntity, ent)){
				node.OccupiedEntity = null;
			}
            entitiesInBoard.Remove(ent);
            var asEnemy = ent as EnemyEntity;
            if(asEnemy) enemyEntities.Remove(asEnemy);
            if(ReferenceEquals(ent, player)) player = null;
		}
		public PlayerEntity GetPlayer() => player;
		public List<EnemyEntity> GetEnemies() => enemyEntities;
		public List<TileEntity> GetAllEntities() => entitiesInBoard;

		public Node GetNodeWithEntity(TileEntity ent) => _nodeInBoard.FirstOrDefault(n => n.OccupiedEntity == ent);
		public Node GetNodeAtPosition(Vector2 pos) => _nodeInBoard.FirstOrDefault(n => n.GridPos == pos);
        public List<Node> AllNode => _nodeInBoard;
		#endregion
	}
}
