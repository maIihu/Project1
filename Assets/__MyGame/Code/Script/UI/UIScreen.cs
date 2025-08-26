using UnityEngine;

namespace __MyGame.Code.Script.UI
{
    public abstract class UIScreen : MonoBehaviour, IUIScreen
    {
        public virtual void Show() => this.gameObject.SetActive(true);
        public virtual void Hide() => this.gameObject.SetActive(false);
    }
}