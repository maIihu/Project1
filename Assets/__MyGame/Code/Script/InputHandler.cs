using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class InputHandler : MonoBehaviour
    {
        private GameplayManager _gameplay;

        private void Start()
        {
            _gameplay = GameplayManager.Instance;
        }
		private void TryShift(Vector2 dir)
		{
			_gameplay.GameLogic.Shift(dir);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow)) TryShift(Vector2.left);
			if (Input.GetKeyDown(KeyCode.RightArrow)) TryShift(Vector2.right);
			if (Input.GetKeyDown(KeyCode.UpArrow)) TryShift(Vector2.up);
			if (Input.GetKeyDown(KeyCode.DownArrow)) TryShift(Vector2.down);
		}
	}
}