using UnityEngine;

namespace __MyGame.Code.Script
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

        public float mapDifficulty;

        public int stepsToNextLevel;
    }
}