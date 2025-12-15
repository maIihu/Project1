using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnEnemyTratCoolDown
{
	public bool CanUse();
	public void ReduceCoolDown();

}
