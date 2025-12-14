using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffectRunner : MonoBehaviour
{
	public void ApplyAll(MapData data,BoardController board)
	{
		if (data == null || data.effects == null) return;
		foreach(var effect in data.effects)
		{
			if(effect) effect.Apply(board,data);	
		}
	}
}
