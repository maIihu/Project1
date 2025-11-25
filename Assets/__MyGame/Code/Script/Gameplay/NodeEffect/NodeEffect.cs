using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEffect : ScriptableObject
{
	[Header("Node Effect Info")]	
	public string effectName;
	public int duration = -1;
	public Sprite effectSpriteOverlay;
	public int priority;
}
