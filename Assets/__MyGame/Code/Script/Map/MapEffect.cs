using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapEffect : ScriptableObject
{
	public abstract void Apply(BoardController board, MapData mapData);
}
