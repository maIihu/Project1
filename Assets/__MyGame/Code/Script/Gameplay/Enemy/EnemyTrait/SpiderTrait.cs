using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiderTrait", menuName = "Enemy/Trait/SpiderTrait")]
public class SpiderTrait : EnemyTrait, IOnAfterMove
{
	[SerializeField] private WebNodeEffect webEffect;
	[SerializeField] private int webDuration = -1;
	public void OnAfterMove(BoardController board, EnemyEntity self, Node from, Node to)
	{
		if (from == null && to == null) return;
		if(from == to) return;	
		from.AddEffect(webEffect,webDuration);
	}
}
