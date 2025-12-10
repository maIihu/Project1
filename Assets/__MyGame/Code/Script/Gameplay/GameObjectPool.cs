using __MyGame.Code.Script;
using UnityEngine;
using UnityEngine.Serialization;
using uPools;


public class GameObjectPool : MonoBehaviour
{
    [Header("----------MAIN CHARACTER----------")]
    [SerializeField] private Transform playerContainer;
    [SerializeField] private PlayerEntity playerPrefab;
    
    [Header("----------ENEMY----------")]
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private EnemyEntity enemyPrefab;
    
    
    [Header("----------OBSTACLE----------")]
    [SerializeField] private Transform obstacleContainer;
    [SerializeField] private ObstacleEntity obstaclePrefab;
    
    [Header("----------NODE----------")]
    [SerializeField] private Transform nodeContainer;
    [SerializeField] private Node nodePrefab;
    
    
    public void InitObjectPooling()
    {
        SharedGameObjectPool.Prewarm(nodePrefab.gameObject, 50, nodeContainer);
        SharedGameObjectPool.Prewarm(playerPrefab.gameObject, 1, playerContainer);
        SharedGameObjectPool.Prewarm(enemyPrefab.gameObject, 1, enemyContainer);
        SharedGameObjectPool.Prewarm(obstaclePrefab.gameObject, 1, obstacleContainer);
    }

    public Node GetNode()
    {
        SharedGameObjectPool.Rent(nodePrefab.gameObject, nodeContainer).TryGetComponent(out Node node);
        return node;
    }
    
    public PlayerEntity GetPlayer()
    {
        SharedGameObjectPool.Rent(playerPrefab.gameObject, playerContainer).TryGetComponent(out PlayerEntity player);
        return player;
    }

    public EnemyEntity GetEnemy()
    {
        SharedGameObjectPool.Rent(enemyPrefab.gameObject, enemyContainer).TryGetComponent(out EnemyEntity enemy);
        return enemy;
    }

    public ObstacleEntity GetObstacle()
    {
        SharedGameObjectPool.Rent(obstaclePrefab.gameObject, obstacleContainer).TryGetComponent(out ObstacleEntity obstacle);
        return obstacle;
    }
    
}
