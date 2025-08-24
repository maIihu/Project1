using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __MyGame.Code.Script
{
    public class GameLogic
    {
        private BoardController _board;

        public GameLogic(BoardController board)
        {
            _board = board;
        }
        
        public void Shift(Vector2 dir)
        {
            var ordererBlocks = OrderBlocksByDirection(_board.GetBlockInBoard(), dir);
            Debug.Log("--- Block Can Move: " + ordererBlocks.Count);
            foreach (var block in ordererBlocks)
            {
                Move(block, dir);
            }
        }
        
        private static List<Block> OrderBlocksByDirection(List<Block> blocks, Vector2 dir)
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
                if(!possibleNode) break;
                if (possibleNode.OccupiedBlock)
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