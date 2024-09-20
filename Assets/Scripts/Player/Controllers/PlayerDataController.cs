using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;
using static GameEnums;

public class PlayerDataController : BaseSingleton<PlayerDataController>
{
    [SerializeField] PlayerBagController _pCoin;
    [SerializeField] PlayerHealthManager _pHealth;

    protected override void Awake()
    {
        base.Awake();
        GetPlayerData();
        DontDestroyOnLoad(gameObject);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnFinishLevel, SaveData);
    }

    private void GetPlayerData()
    {
        string filePath = Application.dataPath + PLAYER_DATA_PATH;
        PlayerData playerData = JSONDataHelper.LoadFromJSon<PlayerData>(filePath);
        _pCoin.SilverCoin = playerData.SilverCoin;
        _pCoin.GoldCoin = playerData.GoldCoin;
        _pHealth.CurrentHP = playerData.HealthPoint;
        _pHealth.MaxHP = playerData.MaxHealthPoint;
    }

    private void OnDestroy()
    {
        //khi tắt ứng dụng sẽ đồng loạt lưu hết tất cả data
        PlayerData pData = new(_pHealth.CurrentHP, _pHealth.MaxHP, _pCoin.SilverCoin, _pCoin.GoldCoin);
        JSONDataHelper.SaveToJSon<PlayerData>(pData, Application.dataPath + PLAYER_DATA_PATH);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnFinishLevel, SaveData);
    }

    private void SaveData(object obj)
    {
        ResultParam pr = obj as ResultParam;
        PlayerData pData = new(_pHealth.CurrentHP, _pHealth.MaxHP, _pCoin.SilverCoin, _pCoin.GoldCoin);
        JSONDataHelper.SaveToJSon<PlayerData>(pData, Application.dataPath + PLAYER_DATA_PATH);
    }
}
