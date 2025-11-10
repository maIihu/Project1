using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __MyGame.Code.Script
{
	public class InputHandler : MonoBehaviour
	{
		public PiercingShoot testPiercingShot;
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
			//_gameplay.GameLogic.Shift(dir);
			StartCoroutine(BoardController.Instance.ShiftAnimated(dir));
		}

		private void Update()
		{
			if (_gameplay == null || _board == null || _logic == null) return;
			Vector2Int aim = Vector2Int.zero;
			if (Input.GetKeyDown(KeyCode.L)) aim = Vector2Int.right;
			if (Input.GetKeyDown(KeyCode.J)) aim = Vector2Int.left;
			if (Input.GetKeyDown(KeyCode.I)) aim = Vector2Int.up;
			if (Input.GetKeyDown(KeyCode.K)) aim = Vector2Int.down;

			if (aim != Vector2Int.zero && testPiercingShot != null)
			{
				var player = _board.player;
				if (player != null)
				{
					var ctx = new AbilityContext
					{
						board = _board,
						gameLogic = _logic,
						direction = aim
					};
					_gameplay.QueueAbility(player, testPiercingShot, ctx);
					_logic.Shift(aim);
				}
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow)) TryShift(Vector2.left);
			if (Input.GetKeyDown(KeyCode.RightArrow)) TryShift(Vector2.right);
			if (Input.GetKeyDown(KeyCode.UpArrow)) TryShift(Vector2.up);
			if (Input.GetKeyDown(KeyCode.DownArrow)) TryShift(Vector2.down);
		}
	}
}