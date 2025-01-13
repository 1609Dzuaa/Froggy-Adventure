using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;
using static GameConstants;
using System;

[Serializable]
public struct LevelHighlight
{
    public Image LevelImage;
    public Image SelectedFrame;
}

public class ItemLevel : MonoBehaviour
{
    [SerializeField] GameObject _lockLevel;
    [SerializeField] Image _imgUnlock;
    [SerializeField] TextMeshProUGUI _txtLevelOrder;
    public LevelHighlight LvlHighlight;
    [HideInInspector] public LevelStaticData LvlSData;
    [HideInInspector] public LevelProgressData LvlPData;
    DataDetail _dataDetail;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnUpdateLevel, HandleUpdateLevel);
        HandleUpdateDataLevel();
        _txtLevelOrder.text = LvlSData.OrderID.ToString();
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnUpdateLevel, HandleUpdateLevel);
    }

    private void HandleUpdateLevel(object obj = null)
    {
        HandleUpdateDataLevel();
    }

    private void HandleUpdateDataLevel()
    {
        string itemFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + LvlSData.OrderID.ToString() + ".json";
        LvlPData = JSONDataHelper.LoadFromJSon<LevelProgressData>(itemFilePath);
        _lockLevel.SetActive(!LvlPData.IsUnlock);
        _imgUnlock.gameObject.SetActive(LvlPData.IsUnlock);
        _dataDetail = new(LvlSData, LvlPData);
        //if (LvlSData.OrderID == 1)
            //Debug.Log("lvl1PData: " + LvlPData.TimeCompleted);
    }

    public void OnClick()
    {
        EventsManager.Instance.NotifyObservers(EEvents.LevelOnSelected, _dataDetail);
    }
}
