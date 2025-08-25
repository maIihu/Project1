using UnityEngine;

namespace __MyGame.Code.Script.Board
{
    public enum MapType
    {
        Green, Blue, Red
    }
    [CreateAssetMenu()]
    public class MapData : ScriptableObject
    {
        public MapType mapType;
        public Sprite sprite1;
        public Sprite sprite2;
    }
}