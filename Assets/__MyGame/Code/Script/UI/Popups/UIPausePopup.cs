namespace __MyGame.Code.Script.UI.Popups
{
    public class UIPausePopup : UIPopupBase
    {
        public override void Show()
        {
            this.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}