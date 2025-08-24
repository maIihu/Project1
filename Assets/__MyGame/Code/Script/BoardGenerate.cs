using System;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class BoardGenerate : MonoBehaviour
    {
        [SerializeField] private Node nodePrefab;
        [SerializeField] private Block blockPrefab;
        
        [SerializeField] private MapData[] mapDataArray;
        
        public static readonly int BoardSize = 6;

        private void Start()
        {
            SpawnMapWithType(MapType.Green);
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
                    var node = Instantiate(nodePrefab, positionToSpawn, Quaternion.identity, this.transform);
                    node.SetSpriteNode((i + j) % 2 == 0 ? mapData.sprite1 : mapData.sprite2);
                }
            }
        }
    }
}
