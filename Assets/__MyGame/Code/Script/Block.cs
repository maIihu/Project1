using UnityEngine;

namespace __MyGame.Code.Script
{
    public class Block : MonoBehaviour
    {
        public Vector2 Pos => this.transform.position;
        public int moveStep;
    }
}