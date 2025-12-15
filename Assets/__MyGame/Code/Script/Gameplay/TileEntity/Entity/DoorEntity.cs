
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

        var player = BoardController.Instance.player.sprite;
        Transform door = this.transform; 
        
        DOTween.Kill(player.transform);

        Sequence seq = DOTween.Sequence();

        seq.Append(player.transform.DOMove(door.position, 1f).SetEase(Ease.InOutSine));
        seq.Join(player.transform.DOScale(Vector3.one * 0.5f, 1f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            GameplayManager.Instance.ReloadAllLevel();
        });
    }

}
