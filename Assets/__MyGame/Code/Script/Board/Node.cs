using UnityEngine;

namespace __MyGame.Code.Script.Board
{
    public class Node : MonoBehaviour
    {
        public Block OccupiedBlock { get; set; }
        public Vector2 GridPos => transform.position;

        public void SetSpriteNode(Sprite sprite)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}