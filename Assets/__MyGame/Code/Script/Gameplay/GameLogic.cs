using System.Collections.Generic;
using System.Linq;
using __MyGame.Code.Script.Board;
using UnityEngine;

namespace __MyGame.Code.Script.Gameplay
{
    public class GameLogic
    {
        private readonly BoardController _board;

        public GameLogic(BoardController board)
        {
            _board = board;
        }
        
        public void Shift(Vector2 dir)
        {
            var orderedBlocks = OrderBlocksByDirection(_board.GetBlocks(), dir);
            Debug.Log("--- Block Can Move: " + orderedBlocks.Count);
            foreach (var block in orderedBlocks)
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
            int remainingStep = block.moveStep;
            while (remainingStep > 0)
            {
                var nextNodeCandidate = _board.GetNodeAtPosition(nextNode.GridPos + dir);
                if(!nextNodeCandidate) break;
                if (nextNodeCandidate.OccupiedBlock)
                {
                    // Attack Block 
                    Debug.Log(block.name + " Can attack " + nextNodeCandidate.OccupiedBlock.name);
                    break;
                }
                nextNode = nextNodeCandidate;
                remainingStep--;
            }
            nextNode.OccupiedBlock = block;
            block.transform.position = nextNode.GridPos;
        }
    }
}