using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;
using static GameEnums;

public class PlayerDataController : BaseSingleton<PlayerDataController>
{
    [SerializeField] PlayerBagController _pBag;
    [SerializeField] PlayerHealthManager _pHealth;
    List<Fruits> _listFruits = new();
    FruitsIventory _fInventoryInstance;

    protected override void Awake()
    {
        base.Awake();
        GetPlayerData();
        DontDestroyOnLoad(gameObject);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnSavePlayerData, SaveData);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnLockLimitedSkills, LockLimitedSkills);
    }

    private void GetPlayerData()
    {
        string filePath = Application.dataPath + PLAYER_DATA_PATH;
        PlayerData playerData = JSONDataHelper.LoadFromJSon<PlayerData>(filePath);
        _fInventoryInstance = JSONDataHelper.LoadFromJSon<FruitsIventory>(Application.dataPath + FRUITS_DATA_PATH);
        _pBag.SilverCoin = playerData.SilverCoin;
        _pBag.GoldCoin = playerData.GoldCoin;
        _pHealth.CurrentHP = playerData.HealthPoint;
        _pHealth.MaxHP = playerData.MaxHealthPoint;
    }

    private void OnDestroy()
    {
        //SavePlayerData();
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnSavePlayerData, SaveData);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnLockLimitedSkills, LockLimitedSkills);
        //Debug.Log("Ondes");
    }

    private void SavePlayerData()
    {
        PlayerData pData = new(_pHealth.CurrentHP, _pHealth.MaxHP, _pBag.SilverCoin, _pBag.GoldCoin);
        JSONDataHelper.SaveToJSon<PlayerData>(pData, Application.dataPath + PLAYER_DATA_PATH);

        _listFruits.Clear();
        foreach (var item in _pBag.DictFruits)
            _listFruits.Add(item.Value);

        _fInventoryInstance.Fruits = _listFruits;
        JSONDataHelper.SaveToJSon<FruitsIventory>(_fInventoryInstance, Application.dataPath + FRUITS_DATA_PATH);
    }

    private void SaveData(object obj)
    {
        if (obj != null)
        {
            ResultParam pr = obj as ResultParam;
            _pBag.SilverCoin += pr.SilverCollected;
            _pBag.GoldCoin += pr.GoldCollected;
        }

        SavePlayerData();
        EventsManager.Instance.NotifyObservers(EEvents.OnItemEligibleCheck);
        Debug.Log("check");
    }

    private void LockLimitedSkills(object obj)
    {
        string filePath = Application.dataPath + SKILLS_DATA_PATH;
        SkillsController sC = JSONDataHelper.LoadFromJSon<SkillsController>(filePath);
        foreach (var s in sC.skills)
            if (s.IsLimited)
                s.IsUnlock = false;
        JSONDataHelper.SaveToJSon<SkillsController>(sC, filePath);
        //Debug.Log("Lock skills");
    }
}
