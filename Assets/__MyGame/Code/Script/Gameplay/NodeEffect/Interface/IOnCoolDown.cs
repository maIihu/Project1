using __MyGame.Code.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnCoolDown
{
	public void ReduceCoolDown(Node node);
	public bool isActive(Node node);
}
