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
        
        [SerializeField] private GameObject chooseCharacterContainer;
        [SerializeField] private Button chooseCharacterButton;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
            chooseCharacterButton.onClick.AddListener(OnChooseCharacterButtonClick);
            
            mainContainer.SetActive(true);
            chooseCharacterContainer.SetActive(false);
        }

        private void OnChooseCharacterButtonClick()
        {
            SceneManager.LoadScene("GameplayScene");
        }

        private void OnPlayButtonClick()
        {
            mainContainer.SetActive(false);
            chooseCharacterContainer.SetActive(true);
        }
        
        
    }
}