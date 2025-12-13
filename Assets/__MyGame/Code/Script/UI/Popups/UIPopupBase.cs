using UnityEngine;

namespace __MyGame.Code.Script.UI.Popups
{
    public abstract class UIPopupBase : MonoBehaviour
    {
        public abstract void Init();
        public abstract void Show();
        public abstract void Hide();
    }
}