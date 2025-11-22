
using __MyGame.Code.Script;
using UnityEngine;
using uPools;

public class AttackEffect : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        SharedGameObjectPool.Return(this.gameObject);
    }
}
