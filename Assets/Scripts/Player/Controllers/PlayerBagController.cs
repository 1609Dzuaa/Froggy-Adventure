using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using DG.Tweening;
using System;

public class FruitCollectedData
{
    public int Count;
    public Sprite FruitImage;
    public EFruits FruitName;

    public FruitCollectedData(int count, Sprite fruitImage)
    {
        Count = count;
        FruitImage = fruitImage;
    }
}

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
    Dictionary<ECurrency, CurrencyInfoComponents> _dictCurrencyInfoComps;
    //2 dictionary này phục vụ việc unlock fruit trong file
    public Dictionary<EFruits, Fruits> DictFruits;
    public Dictionary<EFruits, Fruits> TempDictFruits;
    //dictionary này phục vụ việc ghi lại kết quả bên popupresult
    public Dictionary<EFruits, FruitCollectedData> DictFruitsCollected;

    [HideInInspector] public int SilverCoin;
    [HideInInspector] public int GoldCoin;

    // Start is called before the first frame update
    void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnBuyShopItem, HandleBuyShopItem);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnHandleLevelCompleted, HandleFinishLevel);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnCollectFruit, CollectFruit);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnResetLevel, HandleReset);
    }

    public void SetupDictionary()
    {
        _dictCurrencyInfoComps = new();
        DictFruitsCollected = new();
        foreach (var item in _listCurrencyInfoComps)
            if (!_dictCurrencyInfoComps.ContainsKey(item.Currency))
                _dictCurrencyInfoComps.Add(item.Currency, item);
        DictFruits = new();
        TempDictFruits = new();
        string filePath = Application.persistentDataPath + FRUITS_DATA_PATH;
        FruitsIventory fI = JSONDataHelper.LoadFromJSon<FruitsIventory>(filePath);
        foreach (var fruit in fI.Fruits)
        {
            DictFruits.Add(fruit.FruitName, fruit);
            TempDictFruits.Add(fruit.FruitName, fruit);
        }
        DisplayCurrencyTexts();
    }

    private void DisplayCurrencyTexts()
    {
        _dictCurrencyInfoComps[ECurrency.Silver].TxtPrice.text = SilverCoin.ToString();
        _dictCurrencyInfoComps[ECurrency.Gold].TxtPrice.text = GoldCoin.ToString();
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnBuyShopItem, HandleBuyShopItem);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnHandleLevelCompleted, HandleFinishLevel);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnCollectFruit, CollectFruit);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnResetLevel, HandleReset);
    }

    private void HandleBuyShopItem(object obj)
    {
        ItemShop item = (ItemShop)obj;
        int itemSCoinPrice = item.ItemSData.DictPriceInfo[ECurrency.Silver].Price;
        int itemGCoinPrice = 0;
        if (item.ItemSData.DictPriceInfo.ContainsKey(ECurrency.Gold))
            itemGCoinPrice = item.ItemSData.DictPriceInfo[ECurrency.Gold].Price;

        if (SilverCoin >= itemSCoinPrice && GoldCoin >= itemGCoinPrice)
        {
            //đủ tiền thì mới gọi hàm xử lý mua item đó
            if (item.HandleBuyItem())
            {
                TweenTextCoins(itemSCoinPrice, itemGCoinPrice);
                TweenIcon(itemGCoinPrice);
                SoundsManager.Instance.PlaySfx(ESoundName.BountyAppearVfxSfx, 1.0f);
            }
            //Debug.Log("Mua thanh cong item: " + itemSData.ItemName);
        }
        else
        {
            string content = "Purchase Fail,\nNot Enough Coins!";
            NotificationParam param = new(content, true);
            HandlePurchaseFailed(param);
        }
    }

    private void HandleFinishLevel(object obj)
    {
        ResultParam pr = (ResultParam)obj;
        SilverCoin += pr.SilverCollected;
        GoldCoin += pr.GoldCollected;
        foreach (var item in DictFruits)
            item.Value.FruitCount = TempDictFruits[item.Key].FruitCount;
        DisplayCurrencyTexts();
        //Debug.Log("Finish");
    }

    private void CollectFruit(object obj)
    {
        //ăn trái thì add vào cái dictionary tạm
        //khi nào thực sự có result game thì mới add vào cái fruit riel
        FruitData fruitData = (FruitData)obj;
        TempDictFruits[fruitData.FruitName].FruitCount++;
        if (!DictFruitsCollected.ContainsKey(fruitData.FruitName))
        {
            FruitCollectedData data = new(1, fruitData.FruitImage);
            DictFruitsCollected.Add(fruitData.FruitName, data);
        }
        else
            DictFruitsCollected[fruitData.FruitName].Count++;
        //Debug.Log(DictFruits.Values);
    }

    private void HandleReset(object obj)
    {
        DictFruitsCollected.Clear();
    }

    private void TweenTextCoins(int sCoinPrice, int gCoinPrice)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(
        DOTween.To(() => SilverCoin, x => SilverCoin = x, SilverCoin - sCoinPrice, _duration).OnUpdate(() =>
        {
            DisplayCurrencyTexts();
            //Debug.Log("x: " + SilverCoin);
        }).SetEase(_ease)).Join(
        DOTween.To(() => GoldCoin, x => GoldCoin = x, GoldCoin - gCoinPrice, _duration).OnUpdate(() =>
        {
            DisplayCurrencyTexts();
        }).SetEase(_ease)).OnComplete(() =>
                        EventsManager.Instance.NotifyObservers(EEvents.OnSavePlayerData)
        );
    }

    private void TweenIcon(int goldPrice)
    {
        _dictCurrencyInfoComps[ECurrency.Silver].ImgCurrency.transform.DOShakePosition(_shakeDuration, _shakeIntensity);//.SetEase(_shakeEase);
        if (goldPrice > 0)
            _dictCurrencyInfoComps[ECurrency.Gold].ImgCurrency.transform.DOShakePosition(_shakeDuration, _shakeIntensity);//.SetEase(_shakeEase);
        //Debug.Log("tween icon");
    }

    private void HandlePurchaseFailed(NotificationParam param)
    {
        ShowNotificationHelper.ShowNotification(param);
    }
}
