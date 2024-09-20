using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using DG.Tweening;

public class PlayerBagController : MonoBehaviour
{
    [SerializeField] List<CurrencyInfoComponents> _listCurrencyInfoComps;
    [Header("Dùng cho Tween Shake của Icon Currency")]
    [SerializeField] float _shakeIntensity;
    [SerializeField] float _shakeDuration;
    [SerializeField] Ease _shakeEase;

    [Header("Dùng cho Tween Text")]
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;
    Dictionary<ECurrency, CurrencyInfoComponents> _dictCurrencyInfoComps = new();

    [HideInInspector] public int SilverCoin;
    [HideInInspector] public int GoldCoin;

    // Start is called before the first frame update
    void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnBuyShopItem, HandleBuyShopItem);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnFinishLevel, HandleFinishLevel);
        SetupDictionary();
        DisplayCurrencyTexts();
    }

    private void SetupDictionary()
    {
        foreach (var item in _listCurrencyInfoComps)
            if (!_dictCurrencyInfoComps.ContainsKey(item.Currency))
                _dictCurrencyInfoComps.Add(item.Currency, item);
    }

    private void GetPlayerCoins()
    {
        string filePath = Application.dataPath + PLAYER_DATA_PATH;
        PlayerData playerData = JSONDataHelper.LoadFromJSon<PlayerData>(filePath);
        SilverCoin = playerData.SilverCoin;
        GoldCoin = playerData.GoldCoin;
    }

    private void DisplayCurrencyTexts()
    {
        _dictCurrencyInfoComps[ECurrency.Silver].TxtPrice.text = SilverCoin.ToString();
        _dictCurrencyInfoComps[ECurrency.Gold].TxtPrice.text = GoldCoin.ToString();
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnBuyShopItem, HandleBuyShopItem);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnFinishLevel, HandleFinishLevel);
    }

    private void HandleBuyShopItem(object obj)
    {
        ItemShop item = (ItemShop)obj;
        //ItemStaticData itemSData = (ItemStaticData)obj;
        int itemSCoinPrice = item.ItemSData.DictPriceInfo[ECurrency.Silver].Price;
        int itemGCoinPrice = 0;
        if (item.ItemSData.DictPriceInfo.ContainsKey(ECurrency.Gold))
            itemGCoinPrice = item.ItemSData.DictPriceInfo[ECurrency.Gold].Price;

        if (SilverCoin >= itemSCoinPrice && GoldCoin >= itemGCoinPrice)
        {
            //chỉ khi pass cổng thì mới gọi hàm xử lý mua item đó
            if (item.HandleBuyItem())
            {
                TweenTextCoins(itemSCoinPrice, itemGCoinPrice);
                TweenIcon(itemGCoinPrice);
                SoundsManager.Instance.PlaySfx(ESoundName.CollectHPSfx, 1.0f);
            }
            //Debug.Log("Mua thanh cong item: " + itemSData.ItemName);
        }
        else
        {
            string content = "Purchase Fail,\nNot Enough Coins!";
            NotificationParam param = new(content, true, false, null, null, null);
            HandlePurchaseFailed(param);
        }
    }

    private void HandleFinishLevel(object obj)
    {
        ResultParam pr = (ResultParam)obj;
        SilverCoin += pr.SilverCollected;
        GoldCoin += pr.GoldCollected;
    }

    private void TweenTextCoins(int sCoinPrice, int gCoinPrice)
    {
        DOTween.To(() => SilverCoin, x => SilverCoin = x, SilverCoin - sCoinPrice, _duration).OnUpdate(() =>
        {
            DisplayCurrencyTexts();
            //Debug.Log("x: " + SilverCoin);
        }).SetEase(_ease);

        DOTween.To(() => GoldCoin, x => GoldCoin = x, GoldCoin - gCoinPrice, _duration).OnUpdate(() =>
        {
            DisplayCurrencyTexts();
        }).SetEase(_ease);
    }

    private void TweenIcon(int goldPrice)
    {
        _dictCurrencyInfoComps[ECurrency.Silver].ImgCurrency.transform.DOShakePosition(_shakeDuration, _shakeIntensity);//.SetEase(_shakeEase);
        if (goldPrice > 0)
            _dictCurrencyInfoComps[ECurrency.Gold].ImgCurrency.transform.DOShakePosition(_shakeDuration, _shakeIntensity);//.SetEase(_shakeEase);
        //Debug.Log("tween icon");
    }

    private bool PurchaseItem(ItemStaticData itemSData, ref bool isSpecialItem)
    {
        bool isPurchaseSuccess = true;

        //vì lượng Item ít nên xử lý sw case
        switch (itemSData.ItemName)
        {
            case "HP":
                if (PlayerHealthManager.Instance.CurrentHP == 3)
                {
                    string content = "Purchase Fail,\nMaximum Health Point Reached!";
                    NotificationParam param = new(content, true, false, null, null, null);
                    HandlePurchaseFailed(param);
                    isPurchaseSuccess = false;
                }
                PlayerHealthManager.Instance.CurrentHP = Mathf.Clamp(++PlayerHealthManager.Instance.CurrentHP, 0, 3);
                break;

            case "Temp HP":

                break;

            case "Blue Potion":
                isPurchaseSuccess = HandleUnlockSkill(ESkills.DoubleJump);
                if (isPurchaseSuccess)
                {
                    EventsManager.Instance.NotifyObservers(EEvents.OnUnlockSkill, itemSData);
                    UIManager.Instance.TogglePopup(EPopup.Ability, true);
                    isSpecialItem = true;
                }
                break;

            case "Red Potion":
                isPurchaseSuccess = HandleUnlockSkill(ESkills.WallSlide);
                if (isPurchaseSuccess)
                {
                    EventsManager.Instance.NotifyObservers(EEvents.OnUnlockSkill, itemSData);
                    UIManager.Instance.TogglePopup(EPopup.Ability, true);
                    isSpecialItem = true;
                }
                break;

            case "Green Potion":
                isPurchaseSuccess = HandleUnlockSkill(ESkills.Dash);
                if (isPurchaseSuccess)
                {
                    EventsManager.Instance.NotifyObservers(EEvents.OnUnlockSkill, itemSData);
                    UIManager.Instance.TogglePopup(EPopup.Ability, true);
                    isSpecialItem = true;
                }
                break;
        }

        return isPurchaseSuccess;
    }    

    private bool HandleUnlockSkill(ESkills skillName)
    {
        bool isUnlockSuccess = true;
        string filePath = Application.dataPath + SKILLS_DATA_PATH;
        SkillsController sC = JSONDataHelper.LoadFromJSon<SkillsController>(filePath);
        foreach (var skill in sC.skills)
        {
            if (skill.SkillName == skillName)
            {
                if (skill.IsUnlock)
                {
                    string content = "Purchase Failed,\n" + skillName.ToString() + " Already Unlock!";
                    NotificationParam param = new(content, true, false, null, null, null);
                    HandlePurchaseFailed(param);
                    return !isUnlockSuccess;
                }

                skill.IsUnlock = true;
                break;
            }
        }
        JSONDataHelper.SaveToJSon<SkillsController>(sC, filePath);

        return isUnlockSuccess;
    }

    private void HandlePurchaseFailed(NotificationParam param)
    {
        ShowNotificationHelper.ShowNotification(param);
    }
}
