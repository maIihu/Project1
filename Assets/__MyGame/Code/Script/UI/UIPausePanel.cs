using System;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIPausePanel : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button resumeButton;
        
        private bool _isVisible;

        private void Awake()
        {
            closeButton.onClick.AddListener(ClosePausePanel);
            resumeButton.onClick.AddListener(ClosePausePanel);
        }

        private void ClosePausePanel()
        {
            Toggle(false);
        }

        private void Start()
        {
            Toggle(false);
        }

        public void Toggle(bool active)
        {
            _isVisible = active;
            this.gameObject.SetActive(_isVisible);
        }
    }
}