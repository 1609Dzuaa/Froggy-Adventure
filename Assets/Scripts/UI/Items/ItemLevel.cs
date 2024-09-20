using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;

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
        _lockLevel.SetActive(!LvlPData.IsUnlock);
        _imgUnlock.gameObject.SetActive(LvlPData.IsUnlock);
        _txtLevelOrder.text = LvlSData.OrderID.ToString();
        _dataDetail = new(LvlSData, LvlPData);
    }

    public void OnClick()
    {
        EventsManager.Instance.NotifyObservers(EEvents.LevelOnSelected, _dataDetail);
    }
}
