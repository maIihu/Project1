using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIPauseScreen : UIScreen
    {
        [SerializeField] private Button optionButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;

        public void Init(UIPausePanelController controller)
        {
            optionButton.onClick.AddListener(controller.ShowOptions);
            resumeButton.onClick.AddListener(controller.TogglePanel);
        }

    }
}