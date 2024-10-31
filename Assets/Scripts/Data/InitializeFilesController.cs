using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;
using static GameEnums;

public class InitializeFilesController : MonoBehaviour
{
    [SerializeField] PlayerDataController _pData;
    [SerializeField] ShopController _shopController;
    [SerializeField] PlayerBagController _bagController;

    private void Awake()
    {
        _shopController.CreateItemAndInitFiles();
        _pData.InitializePlayerData(); //có data r mới setup dict
        _bagController.SetupDictionary();
        EventsManager.Instance.NotifyObservers(EEvents.OnItemEligibleCheck);
    }
}
