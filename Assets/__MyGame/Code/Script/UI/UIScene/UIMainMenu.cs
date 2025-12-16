using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private Button playButton;
        
        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
            
            AudioManager.Instance.PlayMusic("BG_Menu", 0.8f, true);
        }

        private void OnPlayButtonClick()
        {
            SceneManager.LoadScene("ChooseCharacter");
        }
        
        
    }
}