using System;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIPausePanelController : MonoBehaviour
    {
        [SerializeField] private UIPauseScreen pauseScreen;
        [SerializeField] private UIOptionScreen optionScreen;
        
        [SerializeField] private Button closeButton;

        private bool _visible = true;
        
        private void Awake()
        {
            pauseScreen.Init(this);
            optionScreen.Init(this);
            
            closeButton.onClick.AddListener(TogglePanel);
        }

        private void OnEnable()
        {
            ShowPause();
        }
        
        public void ShowPause()
        {
            pauseScreen.Show();
            optionScreen.Hide();
        }

        public void ShowOptions()
        {
            pauseScreen.Hide();
            optionScreen.Show();
        }

        public void TogglePanel()
        {
            _visible = !_visible;
            this.gameObject.SetActive(_visible);
        }
    }
}