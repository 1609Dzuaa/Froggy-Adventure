using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static GameConstants;
using static GameEnums;

public class PlayerDataController : BaseSingleton<PlayerDataController>
{
    public PlayerBagController PlayerBag;
    public PlayerHealthManager PlayerHealth;
    List<Fruits> _listFruits = new();
    FruitsIventory _fInventory;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnSavePlayerData, SaveData);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnLockLimitedSkills, LockLimitedSkills);
    }

    //đc gọi ở script InitializeFiles để đồng nhất việc init các file cùng 1 lúc
    public void InitializePlayerData()
    {
        string filePath = Application.persistentDataPath + PLAYER_DATA_PATH;
        if (!File.Exists(filePath))
        {
            PlayerData pData = new PlayerData(DEFAULT_PLAYER_HP, DEFAULT_PLAYER_HP, DEFAULT_PLAYER_COIN, DEFAULT_PLAYER_COIN);
            JSONDataHelper.SaveToJSon<PlayerData>(pData, filePath);
            GetPlayerData(filePath);
        }
        else
        {
            GetPlayerData(filePath);
        }
    }

    private void GetPlayerData(string filePath)
    {
        PlayerData playerData = JSONDataHelper.LoadFromJSon<PlayerData>(filePath);
        _fInventory = JSONDataHelper.LoadFromJSon<FruitsIventory>(Application.persistentDataPath + FRUITS_DATA_PATH);
        PlayerBag.SilverCoin = playerData.SilverCoin;
        PlayerBag.GoldCoin = playerData.GoldCoin;
        PlayerHealth.CurrentHP = playerData.HealthPoint;
        PlayerHealth.MaxHP = playerData.MaxHealthPoint;
    }

    private void OnDestroy()
    {
        SavePlayerData();
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnSavePlayerData, SaveData);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnLockLimitedSkills, LockLimitedSkills);
        //Debug.Log("Ondes");
    }

    private void SavePlayerData()
    {
        PlayerData pData = new(PlayerHealth.CurrentHP, PlayerHealth.MaxHP, PlayerBag.SilverCoin, PlayerBag.GoldCoin);
        JSONDataHelper.SaveToJSon<PlayerData>(pData, Application.persistentDataPath + PLAYER_DATA_PATH);

        _listFruits.Clear();
        foreach (var item in PlayerBag.DictFruits)
            _listFruits.Add(item.Value);

        _fInventory.Fruits = _listFruits;
        JSONDataHelper.SaveToJSon<FruitsIventory>(_fInventory, Application.persistentDataPath + FRUITS_DATA_PATH);
    }

    private void SaveData(object obj)
    {
        if (obj != null)
        {
            ResultParam pr = obj as ResultParam;
            PlayerBag.SilverCoin += pr.SilverCollected;
            PlayerBag.GoldCoin += pr.GoldCollected;
        }

        SavePlayerData();
        EventsManager.Instance.NotifyObservers(EEvents.OnItemEligibleCheck);
        //Debug.Log("check");
    }

    private void LockLimitedSkills(object obj)
    {
        string filePath = Application.persistentDataPath + SKILLS_DATA_PATH;
        SkillsController sC = JSONDataHelper.LoadFromJSon<SkillsController>(filePath);
        foreach (var s in sC.skills)
            if (s.IsLimited)
                s.IsUnlock = false;
        JSONDataHelper.SaveToJSon<SkillsController>(sC, filePath);
        //Debug.Log("Lock skills");
    }
}
