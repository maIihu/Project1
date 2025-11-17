using UnityEngine;
using UnityEngine.UIElements;

namespace __MyGame.Code.Script.UI.Screens
{
    public class UIGameplayScreen : UIScreenBase
    {
        [SerializeField] private ProgressBarUI turnProgressBar;

        public void UpdateProgress(float value, float maxValue)
        {
            if (value > maxValue) return;
            turnProgressBar.UpdateProgress(value, maxValue);
        }
        
        public override void Show()
        {
            
        }

        public override void Hide()
        {
        }
    }
}