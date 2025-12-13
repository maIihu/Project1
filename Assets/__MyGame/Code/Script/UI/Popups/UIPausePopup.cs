using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI.Popups
{
    public class UIPausePopup : UIPopupBase
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;
        
        public override void Init()
        {
            continueButton.onClick.AddListener(OnContinueButtonClick);
            settingButton.onClick.AddListener(OnSettingButtonClick);
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

        private void OnSettingButtonClick()
        {
            //TODo: open setting popup
        }

        private void OnQuitButtonClick()
        {
            //Todo: open main menu screen
        }
    }
}