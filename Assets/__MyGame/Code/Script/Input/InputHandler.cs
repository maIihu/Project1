using System;
using System.Collections.Generic;
using System.Linq;
using __MyGame.Code.Script.Gameplay;
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

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                _gameplay.GameLogic.Shift(Vector2.left);

            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                _gameplay.GameLogic.Shift(Vector2.right);

            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                _gameplay.GameLogic.Shift(Vector2.up);

            if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                _gameplay.GameLogic.Shift(Vector2.down);
        }
    }
}