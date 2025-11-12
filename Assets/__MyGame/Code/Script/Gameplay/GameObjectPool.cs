using UnityEngine;
using uPools;

namespace __MyGame.Code.Script
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private EnemyEntity enemyPrefab;
        
        public void InitEnemyPooling()
        {
            SharedGameObjectPool.Prewarm(enemyPrefab.gameObject, 1);
        }

        public EnemyEntity GetEnemy(Vector3 position, Quaternion rotation, Transform parent)
        {
            SharedGameObjectPool.Rent(enemyPrefab.gameObject, position, rotation, parent).TryGetComponent(out EnemyEntity enemy);
            return enemy;
        }
    }
}