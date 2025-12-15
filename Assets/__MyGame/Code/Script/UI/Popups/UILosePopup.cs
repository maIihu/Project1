using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI.Popups
{
    public class UILosePopup : UIPopupBase
    {
        [SerializeField] private Button replayButton;
        [SerializeField] private Button quitButton;
        
        public override void Init()
        {
            replayButton.onClick.AddListener(OnReplayButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnReplayButtonClick()
        {
        }

        private void OnQuitButtonClick()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}