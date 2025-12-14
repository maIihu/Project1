
using System.Collections.Generic;
using __MyGame.Code.Script;
using UnityEngine;

public class LevelDesign : MonoBehaviour
{
    [SerializeField] private Node nodePrefab;
    
    private List<Node> _nodeInBoard = new List<Node>();
    
    public void GenerateBoard(MapData mapData, int boardSize)
    {
        ClearBoard();
        var offset = boardSize / 2;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                var positionToSpawn = new Vector3(i - offset, j - offset, 0);
                var node = Instantiate(nodePrefab, positionToSpawn, Quaternion.identity, this.transform);
                node.name = node.GridPos.ToString();
                node.SetBaseSprite((i + j) % 2 == 0 ? mapData.sprite1 : mapData.sprite2);
                _nodeInBoard.Add(node);
            }
        }
    }

    public void ClearBoard()
    {
        foreach (var node in _nodeInBoard)
        {
            Destroy(node.gameObject);
        }
        _nodeInBoard.Clear();
    }
    
    public LevelData ExportLevelData(int levelId, int boardSize)
    {
        var data = new LevelData
        {
            levelId = levelId,
            col = boardSize,
            row = boardSize,
            nodesInLevel = new List<NodeData>()
        };

        foreach (var node in _nodeInBoard)
        {
            data.nodesInLevel.Add(new NodeData
            {
                Position = node.transform.position,
                ImgId = node.GetComponent<SpriteRenderer>().sprite.name
            });
        }

        return data;
    }
    
    public void LoadLevel(LevelData data)
    {
        ClearBoard();

        foreach (var nodeData in data.nodesInLevel)
        {
            var node = Instantiate(nodePrefab, nodeData.Position, Quaternion.identity, transform);
            
            var sprite = Resources.Load<Sprite>("Sprite/" + nodeData.ImgId);
            //node.SetBaseSprite(sprite);
            node.GetComponent<SpriteRenderer>().sprite = sprite;

            _nodeInBoard.Add(node);
        }
    }

    
}
