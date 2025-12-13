
using __MyGame.Code.Script;
using DG.Tweening;
using UnityEngine;

public class DoorEntity : TileEntity
{
    public void InitDoor()
    {
        //maxHP = currentHP = 10000;
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel");
        GameplayManager.Instance.ChangeState(GameState.Pause);
        DOVirtual.DelayedCall(1f, () =>
            {
                GameplayManager.Instance.ReloadAllLevel();
            }
        );
    }
}
