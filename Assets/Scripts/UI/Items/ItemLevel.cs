using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;
using static GameConstants;

public class ItemLevel : MonoBehaviour
{
    [SerializeField] GameObject _lockLevel;
    [SerializeField] Image _imgUnlock;
    [SerializeField] TextMeshProUGUI _txtLevelOrder;
    [HideInInspector] public LevelStaticData LvlSData;
    [HideInInspector] public LevelProgressData LvlPData;
    DataDetail _dataDetail;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnUpdateLevel, HandleUpdateLevel);
        HandleLockLevel();
        _txtLevelOrder.text = LvlSData.OrderID.ToString();
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnUpdateLevel, HandleUpdateLevel);
    }

    private void HandleUpdateLevel(object obj = null)
    {
        if (obj != null)
        {
            //int levelIndex = (int)obj;
            //if (LvlSData.OrderID == levelIndex)
                HandleLockLevel();
        }
    }

    private void HandleLockLevel()
    {
        string itemFilePath = Application.persistentDataPath + LEVEL_DATA_PATH + LvlSData.OrderID.ToString() + ".json";
        LvlPData = JSONDataHelper.LoadFromJSon<LevelProgressData>(itemFilePath);
        _lockLevel.SetActive(!LvlPData.IsUnlock);
        _imgUnlock.gameObject.SetActive(LvlPData.IsUnlock);
        _dataDetail = new(LvlSData, LvlPData);
    }

    public void OnClick()
    {
        EventsManager.Instance.NotifyObservers(EEvents.LevelOnSelected, _dataDetail);
    }
}
