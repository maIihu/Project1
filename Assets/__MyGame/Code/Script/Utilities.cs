using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
}
public enum AbilityType { Passive, Active }
public enum DamageType { Physical, Piercing }

public enum CastPhase { BeforeMove, InsteadOfMove, AfterMove, Reaction }
public enum TargetType { Self, Adjacent, Line, Column }

[System.Serializable]
public class NodeEffectInstance
{
	public NodeEffect effect;
	public int duration;
}
