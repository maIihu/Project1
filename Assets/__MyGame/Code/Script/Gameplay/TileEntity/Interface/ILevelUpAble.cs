using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelUpAble
{
	public void GainExp(int exp);
	public void LevelUp();
}
