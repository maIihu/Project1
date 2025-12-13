using UnityEngine;

namespace __MyGame.Code.Script.UI.Screens
{
    public abstract class UIScreenBase : MonoBehaviour
    {
        public abstract void Init();
        public abstract void Show();
        public abstract void Hide();
    }
}