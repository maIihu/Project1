using System;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private UIPausePanelController pausePanelController;

        private void Awake()
        {
            pauseButton.onClick.AddListener(OnClickPauseButton);
        }

        private void Start()
        {
            pausePanelController.TogglePanel();
        }

        private void OnClickPauseButton()
        {
            pausePanelController.TogglePanel();
        }
    }
}