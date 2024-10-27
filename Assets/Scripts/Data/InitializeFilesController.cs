using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class InitializeFilesController : MonoBehaviour
{
    [SerializeField] PlayerDataController _pData;
    [SerializeField] ShopController _shopController;
    [SerializeField] PlayerBagController _bagController;

    private void Awake()
    {
        _shopController.CreateItemAndInitFiles();
        _bagController.SetupDictionary();
        _pData.InitializePlayerData();
    }
}
