using System;
using System.Collections.Generic;
using __MyGame.Code.Script;
using UnityEngine;

[Serializable]
public class NodeData
{
    public Vector2 Position;
    public string ImgId;
}

[Serializable]
public class LevelData
{
    public int levelId;
    public int col;
    public int row;
    public List<NodeData> nodesInLevel;
}
