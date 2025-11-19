using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __MyGame.Code.Script
{
	public class InputHandler : MonoBehaviour
	{
		private GameplayManager _gameplay;
		private BoardController _board;
		private GameLogic _logic;
		
		private void Start()
		{
			_gameplay = GameplayManager.Instance;
			_board = BoardController.Instance;
			_logic = _gameplay.GameLogic;
		}
		private void TryShift(Vector2 dir)
		{
			//if (BoardController.Instance.IsAnimating) return;
			GameplayManager.Instance.StepMoveCount();
			StartCoroutine(BoardController.Instance.ShiftAnimated(dir));
		}

		private void Update()
		{
			//Debug.Log(GameplayManager.Instance.IsInputLocked);
			if (GameplayManager.Instance.IsInputLocked)
				return;
			if (SkillSelectedUIController.Instance != null && SkillSelectedUIController.Instance.IsSelecting)
				return;

			if (_gameplay == null || _board == null || _logic == null) return;

			if (Input.GetKeyDown(KeyCode.LeftArrow)) TryShift(Vector2.left);
			if (Input.GetKeyDown(KeyCode.RightArrow)) TryShift(Vector2.right);
			if (Input.GetKeyDown(KeyCode.UpArrow)) TryShift(Vector2.up);
			if (Input.GetKeyDown(KeyCode.DownArrow)) TryShift(Vector2.down);
		}
	}
}