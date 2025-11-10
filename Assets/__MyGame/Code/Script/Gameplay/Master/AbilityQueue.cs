using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct QueuedCast
{
	public PlayerEntity user;
	public BaseCharacterAbility ability;
	public AbilityContext context;
}
public class AbilityPipeLine
{
	private readonly Dictionary<CastPhase, List<QueuedCast>> q =
		new Dictionary<CastPhase, List<QueuedCast>>
		{
			{CastPhase.BeforeMove, new List<QueuedCast>() },
			{CastPhase.InsteadOfMove, new List<QueuedCast>() },
			{CastPhase.AfterMove, new List<QueuedCast>() },
			{CastPhase.Reaction, new List<QueuedCast>() },
		};
	public void Queue(QueuedCast c) => q[c.ability.phase].Add(c);

	public List<QueuedCast> Drain(CastPhase phase)
	{
		var list = q[phase];
		var copy = new List<QueuedCast>(list);
		list.Clear();
		return copy;
	}
	public void ClearAll()
	{
		foreach (var kv in q) kv.Value.Clear();
	}
}
