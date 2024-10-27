using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class InitializeFilesController : MonoBehaviour
{
    [SerializeField] PlayerDataController _pData;
    [SerializeField] ShopController _shopController;

    private void Awake()
    {
        _shopController.CreateItemAndInitFiles();
        _pData.InitializePlayerData();
    }
}
