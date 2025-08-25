using UnityEngine;

namespace __MyGame.Code.Script.Board
{
    public class Block : MonoBehaviour
    {
        public Vector2 Pos => this.transform.position;
        public int moveStep;
    }
}