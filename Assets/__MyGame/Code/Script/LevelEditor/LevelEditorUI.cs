using System;
using System.IO;
using System.Linq;
using __MyGame.Code.Script;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelEditorUI : MonoBehaviour
{
    [SerializeField] private LevelDesign levelDesign;
    
    [SerializeField] private Button generateBoardButton;
    [SerializeField] private TMP_InputField boarsSizeInputField;
    [SerializeField] private MapData[] mapDataArr;
    [SerializeField] private TMP_Dropdown dropdown;
    
    [SerializeField] private TMP_InputField levelIdInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    
    private MapData _chosenMapData;
    private int _chosenBoardSize;
    private int _levelId;

    private void Awake()
    {
        generateBoardButton.onClick.AddListener(GenerateBoard);
        saveButton.onClick.AddListener(SaveLevelData);
        loadButton.onClick.AddListener(LoadLevelData);
    }

    private void Start()
    {
        SetupDropdown();

        OnDropdownChanged(0);
        OnInputChanged(boarsSizeInputField.text);

        dropdown.onValueChanged.AddListener(OnDropdownChanged);
        boarsSizeInputField.onEndEdit.AddListener(OnInputChanged);
    }

    private void OnDropdownChanged(int index)
    {
        _chosenMapData = mapDataArr[index];
    }

    private void OnInputChanged(string value)
    {
        int.TryParse(value, out _chosenBoardSize);
    }
    
    private void SetupDropdown()
    {
        dropdown.ClearOptions();

        var options = mapDataArr.Select(t => t.name).ToList();

        dropdown.AddOptions(options);

        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    private void GenerateBoard()
    {
        levelDesign.GenerateBoard(_chosenMapData, _chosenBoardSize);
    }

    private void SaveLevelData()
    {
        _levelId = int.Parse(levelIdInputField.text);
        var levelData = levelDesign.ExportLevelData(_levelId, _chosenBoardSize);

        string json = JsonUtility.ToJson(levelData, true);
        string levelName = "level_"  + _levelId + ".json";
        string path = Application.dataPath + "/Resources/LevelData/" + levelName;

        File.WriteAllText(path, json);

        Debug.Log("Saved level to: " + path);
        levelDesign.ClearBoard();
    }

    private void LoadLevelData()
    {
        _levelId = int.Parse(levelIdInputField.text);

        string levelName = "LevelData/level_" + _levelId; 

        TextAsset json = Resources.Load<TextAsset>(levelName);
        if (json == null)
        {
            Debug.LogError("Can't find level data!");
            return;
        }

        var levelData = JsonUtility.FromJson<LevelData>(json.text);
        levelDesign.LoadLevel(levelData);
    }

}
