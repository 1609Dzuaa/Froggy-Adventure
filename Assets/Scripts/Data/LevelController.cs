using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;

public class LevelController : MonoBehaviour
{
    [SerializeField] ItemLevel _itemLevel;
    LevelStaticData[] _dataLevels;
    ItemLevel[] _arrItemLevels;
    Dictionary<ItemLevel, LevelHighlight> _dictButtonLevels = new();
    private static ProfilerMarker performanceMarker = new ProfilerMarker("ImprovedCode");

    // Start is called before the first frame update
    void Start()
    {
        LoadDataItemLevel();
        CacheLevelProgress(); //cache level progress data into LevelsManager
        SetupDictionary();
        //performanceMarker.Begin();
        SetupForLevelButtons(_arrItemLevels);
        //performanceMarker.End();
    }

    private void OnDisable()
    {
        ResetHighlightButtons();
    }

    private void LoadDataItemLevel()
    {
        //load static & progress data lên
        _dataLevels = Resources.LoadAll<LevelStaticData>("Levels Data");
        if (_dataLevels != null)
        {
            foreach(var item in _dataLevels)
            {
                ItemLevel itemLevel = Instantiate(_itemLevel, transform);
                itemLevel.LvlSData = item;
                string itemFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + item.OrderID.ToString() + ".json";
                if (!File.Exists(itemFilePath))
                {
                    LevelProgressData data = new LevelProgressData(item.OrderID, 
                        (item.OrderID != 1) ? DEFAULT_LEVEL_UNLOCK : true, DEFAULT_LEVEL_COMPLETED,
                        DEFAULT_TIME_COMPLETED);
                    JSONDataHelper.SaveToJSon<LevelProgressData>(data, itemFilePath);
                }
            }
        }
    }

    private void CacheLevelProgress()
    {
        for (int i = 0; i < _dataLevels.Length; i++)
        {
            string itemFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + _dataLevels[i].OrderID.ToString() + ".json";
            LevelProgressData levelData = JSONDataHelper.LoadFromJSon<LevelProgressData>(itemFilePath);
            if (!LevelsManager.Instance.DictLevelsProgress.ContainsKey(_dataLevels[i].OrderID))
                LevelsManager.Instance.DictLevelsProgress.Add(_dataLevels[i].OrderID, levelData);
        }
    }


    private void ResetHighlightButtons(object obj = null)
    {
        foreach (var item in _dictButtonLevels)
        {
            item.Value.LevelImage.color = new(0.45f, 0.45f, 0.45f, 1f);
            item.Value.SelectedFrame.gameObject.SetActive(false);
        }
    }

    private void SetupDictionary()
    {
        _arrItemLevels = GetComponentsInChildren<ItemLevel>();
        foreach (var item in _arrItemLevels)
            if (!_dictButtonLevels.ContainsKey(item))
                _dictButtonLevels.Add(item, item.LvlHighlight);
    }

    private void SetupForLevelButtons(ItemLevel[] buttons)
    {
        foreach (var button in buttons)
            button.GetComponentInChildren<Button>().onClick.AddListener(() => ButtonLevelOnClick(button));
    }

    private void ButtonLevelOnClick(ItemLevel buttonClicked)
    {
        ResetHighlightButtons();
        _dictButtonLevels[buttonClicked].LevelImage.color = new(1f, 1f, 1f, 1f);
        _dictButtonLevels[buttonClicked].SelectedFrame.gameObject.SetActive(true);
    }
}
