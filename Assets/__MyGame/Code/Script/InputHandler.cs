using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class InputHandler : MonoBehaviour
    {
        private BoardController _board;

        private void Start()
        {
            _board = GameplayManager.Instance.board;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                Shift(Vector2.left);

            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                Shift(Vector2.right);

            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                Shift(Vector2.up);

            if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                Shift(Vector2.down);
        }

        private void Shift(Vector2 dir)
        {
            var ordererBlocks = OrderBlocksByDirection(_board.GetBlockInBoard(), dir);
            Debug.Log("--- Block Can Move: " + ordererBlocks.Count);
            foreach (var block in ordererBlocks)
            {
                Move(block, dir);
            }
        }

        private List<Block> OrderBlocksByDirection(List<Block> blocks, Vector2 dir)
        {
            if(dir == Vector2.right)
                return blocks.OrderByDescending(b => b.Pos.x).ToList();
            if(dir == Vector2.left)
                return blocks.OrderBy(b => b.Pos.x).ToList();
            if(dir == Vector2.up)
                return blocks.OrderByDescending(b => b.Pos.y).ToList();
            return blocks.OrderBy(b => b.Pos.y).ToList();
        }

        private void Move(Block block,  Vector2 dir)
        {
            var nextNode = _board.GetNodeWithBlock(block);
            nextNode.OccupiedBlock = null;
            int step = block.moveStep;
            while (step > 0)
            {
                var possibleNode = _board.GetNodeAtPosition(nextNode.GridPos + dir);
                if(possibleNode == null) break;
                if (possibleNode.OccupiedBlock != null)
                {
                    // Attack Block 
                    Debug.Log(block.name + " Can attack " + possibleNode.OccupiedBlock.name);
                    break;
                }
                nextNode = possibleNode;
                step--;
            }
            nextNode.OccupiedBlock = block;
            block.transform.position = nextNode.GridPos;
        }
    }
}