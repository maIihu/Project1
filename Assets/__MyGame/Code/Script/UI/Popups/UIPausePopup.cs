using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI.Popups
{
    public class UIPausePopup : UIPopupBase
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button quitButton;
        
        public override void Init()
        {
            continueButton.onClick.AddListener(OnContinueButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        public override void Show()
        {
            this.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            this.gameObject.SetActive(false);
        }

        private void OnContinueButtonClick()
        {
            this.Hide();
        }
        

        private void OnQuitButtonClick()
        {
            SceneManager.LoadScene("ChooseCharacter");

        }
    }
}