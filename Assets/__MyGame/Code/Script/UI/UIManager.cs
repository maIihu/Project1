using System;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private UIPausePanel pausePanel;

        private void Awake()
        {
            pauseButton.onClick.AddListener(OnClickPauseButton);
        }

        private void OnClickPauseButton()
        {
            pausePanel.Toggle();
        }
    }
}