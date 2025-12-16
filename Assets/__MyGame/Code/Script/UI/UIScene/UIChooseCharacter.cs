
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIChooseCharacter : MonoBehaviour
{
    [SerializeField] private Button chooseCharacterButton;

    private void Start()
    {
        chooseCharacterButton.onClick.AddListener(OnChooseCharacterButtonClick);

    }
    
    private void OnChooseCharacterButtonClick()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
