using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CycledNodeEffect : NodeEffect, IOnNodeEnter, IOnCoolDown, IChangeNodeSprite
{
	public bool startActive;
	public int activeTurn;
	public int cooldownTurns;
	[Header("Sprites")]
	public Sprite activeSprite;
	public Sprite inactiveSprite;

	public virtual void Initial(Node node, NodeEffectInstance inst)
	{
		inst.isActive = startActive;
		inst.stateCounter = (inst.isActive ? activeTurn : cooldownTurns);
	}

	public Sprite GetNodeEffectSprite(Node node, NodeEffectInstance inst, Sprite baseSprite)
	{
		if (inst == null || inst.effect != this) return baseSprite;
		var sprite = inst.isActive ? (activeSprite ? activeSprite : baseSprite) : (inactiveSprite ? inactiveSprite : baseSprite);
		Debug.Log(sprite.name);
		return sprite;
	}

	public bool isActive(Node node)
	{
		return node != null && node.nodeEffect != null && node.nodeEffect.effect == this && node.nodeEffect.isActive;
	}

	public void OnNodeEnter(BoardController board, TileEntity entity, Node node)
	{
		if (entity == null || node == null || node.nodeEffect == null) return;
		if (node.nodeEffect.effect != this) return;

		if (!node.nodeEffect.isActive) return;
		OnNodeEnterEffect(board, entity, node);
	}

	public abstract void OnNodeEnterEffect(BoardController board, TileEntity entity, Node node);

	public void ReduceCoolDown(Node node)
	{
		var inst = node.nodeEffect;
		if (inst == null || inst.effect != this) return;

		if (--inst.stateCounter <= 0)
		{
			inst.isActive = !inst.isActive;
			inst.stateCounter = inst.isActive ? activeTurn : cooldownTurns;
		}
		if (!inst.isActive)
			Debug.Log("Spike not active");
	}
}
