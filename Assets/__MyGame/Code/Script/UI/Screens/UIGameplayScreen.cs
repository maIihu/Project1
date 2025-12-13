using _MyCore.DesignPattern.Observer.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI.Screens
{
    public class UIGameplayScreen : UIScreenBase
    {
        [SerializeField] private ProgressBarUI turnProgressBar;
        [SerializeField] private Button pauseButton;

        public void UpdateProgress(float value, float maxValue)
        {
            if (value > maxValue) return;
            turnProgressBar.UpdateProgress(value, maxValue);
        }

        public override void Init()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }
        
        public override void Show()
        {
        }

        public override void Hide()
        {
            
        }
        
        private void OnPauseButtonClicked()
        {
            UIManager.Instance.ShowPausePopup();
            MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnShowPopup));
        }

    }
}