using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __MyGame.Code.Script.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Node nodePrefab;
        [SerializeField] private Transform nodeContainer;
        
        [SerializeField] private Block blockPrefab;
        [SerializeField] private Transform blockContainer;
        
        [SerializeField] private MapData[] mapDataArray;
        
        public static readonly int BoardSize = 6;

        private List<Node> _nodeInBoard;
        private List<Block> _blockInBoard;

        private void Start()
        {
            _nodeInBoard = new List<Node>();
            _blockInBoard = new List<Block>();
            
            SpawnMapWithType(MapType.Green);
            SpawnBlocksToMap(2);
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

        private void SpawnBlocksToMap(int amount)
        {
            var freeNodes = _nodeInBoard.Where(n => n.OccupiedBlock == null)
                .OrderBy(n => Random.value).ToList();
            foreach (var node in freeNodes.Take(amount))
            {
                var block = Instantiate(blockPrefab, node.transform.position, Quaternion.identity, blockContainer);
                node.OccupiedBlock = block;
                _blockInBoard.Add(block);
            }
        }

        public List<Block> GetBlockInBoard()
        {
            return _blockInBoard;
        }

        public Node GetNodeWithBlock(Block block)
        {
            return _nodeInBoard.FirstOrDefault(node => node.OccupiedBlock == block);
        }

        public Node GetNodeAtPosition(Vector2 pos)
        {
            return _nodeInBoard.FirstOrDefault(n => n.GridPos == pos);
        }
    }
}
