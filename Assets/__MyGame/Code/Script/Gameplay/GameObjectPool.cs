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
    
    [Header("----------ATTACK EFFECT----------")]
    [SerializeField] private Transform attackEffectContainer;
    [SerializeField] private AttackEffect attackEffectPrefab;
    
    [Header("----------OBSTACLE----------")]
    [SerializeField] private Transform obstacleContainer;
    [SerializeField] private ObstacleEntity obstaclePrefab;
    
    [Header("----------NODE----------")]
    [SerializeField] private Transform nodeContainer;
    [SerializeField] private Node nodePrefab;
    
    
    private Vector2 lastDir = Vector2.right;

    private void OnEnable()
    {
        InputHandler.OnInputDirection += UpdateDirection;
    }

    private void OnDisable()
    {
        InputHandler.OnInputDirection -= UpdateDirection;
    }

    private void UpdateDirection(Vector2 dir)
    {
        lastDir = dir;
    }

    
    public void InitObjectPooling()
    {
        SharedGameObjectPool.Prewarm(nodePrefab.gameObject, 30, nodeContainer);
        SharedGameObjectPool.Prewarm(playerPrefab.gameObject, 1, playerContainer);
        SharedGameObjectPool.Prewarm(enemyPrefab.gameObject, 10, enemyContainer);
        SharedGameObjectPool.Prewarm(attackEffectPrefab.gameObject, 10, attackEffectContainer);
        SharedGameObjectPool.Prewarm(obstacleContainer.gameObject, 10, obstacleContainer);
    }

    public Node GetNode(Vector3 position, Quaternion rotation)
    {
        SharedGameObjectPool.Rent(nodePrefab.gameObject, position, rotation).TryGetComponent(out Node node);
        return node;
    }
    
    public PlayerEntity GetPlayer(Vector3 position, Quaternion rotation)
    {
        SharedGameObjectPool.Rent(playerPrefab.gameObject, position, rotation).TryGetComponent(out PlayerEntity player);
        return player;
    }

    public EnemyEntity GetEnemy(Vector3 position, Quaternion rotation)
    {
        SharedGameObjectPool.Rent(enemyPrefab.gameObject, position, rotation).TryGetComponent(out EnemyEntity enemy);
        return enemy;
    }

    public ObstacleEntity GetObstacle(Vector3 position, Quaternion rotation)
    {
        SharedGameObjectPool.Rent(obstaclePrefab.gameObject, position, rotation).TryGetComponent(out ObstacleEntity obstacle);
        return obstacle;
    }

    public AttackEffect GetAttackEffect(Vector3 position)
    {
        float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        
        position = new Vector3(position.x, position.y, position.z);

        SharedGameObjectPool.Rent(attackEffectPrefab.gameObject, position , rot)
            .TryGetComponent(out AttackEffect attackEffect);
        return attackEffect;
    }
}
