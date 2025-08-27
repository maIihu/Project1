using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace __MyGame.Code.Script.UI
{
    public class UIWindowScaleOption : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown windowScaleDropdown;
        [SerializeField] private TextMeshProUGUI value;

        private void Awake()
        {
            windowScaleDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        
        private void OnDropdownValueChanged(int index)
        {
            string selectedOption = windowScaleDropdown.options[index].text;
            value.text = selectedOption;
            Debug.Log("Selected: " + selectedOption);
            Debug.Log("Current Screen: " + Screen.width + "x" + Screen.height);
        }
    }
}