using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIOptionScreen : UIScreen
    {
        [SerializeField] private Button backButton;

        public void Init(UIPausePanelController controller)
        {
            backButton.onClick.AddListener(controller.ShowPause);
        }
    }
}