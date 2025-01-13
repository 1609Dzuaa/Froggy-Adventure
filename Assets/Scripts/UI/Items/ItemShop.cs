using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;

[System.Serializable]
public class CurrencyInfoComponents
{
    public ECurrency Currency;
    public Image ImgCurrency;
    public TextMeshProUGUI TxtPrice;
}

public class ItemShop : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _txtName;
    [SerializeField] protected Image _imgItem;
    [SerializeField] protected List<CurrencyInfoComponents> _listCurrencyInfoComps;
    public ItemStaticData ItemSData;
    protected Dictionary<ECurrency, CurrencyInfoComponents> _dictCurrencyInfoComps = new();
    protected bool _isPurchaseSuccess = true;

    protected virtual void Awake()
    {
        foreach (var item in _listCurrencyInfoComps)
            if (!_dictCurrencyInfoComps.ContainsKey(item.Currency))
                _dictCurrencyInfoComps.Add(item.Currency, item);

        _txtName.text = ItemSData.ItemName;
        _imgItem.sprite = ItemSData.ItemImage;
        foreach (var item in _dictCurrencyInfoComps)
        {
            if (!ItemSData.DictPriceInfo.ContainsKey(item.Key)) return;

            string ItemPrice = ItemSData.DictPriceInfo[item.Key].Price.ToString();
            Sprite ItemImage = ItemSData.DictPriceInfo[item.Key].Image;
            _dictCurrencyInfoComps[item.Key].TxtPrice.text = ItemPrice;
            _dictCurrencyInfoComps[item.Key].ImgCurrency.sprite = ItemImage;

            _dictCurrencyInfoComps[item.Key].TxtPrice.gameObject.SetActive(true);
            _dictCurrencyInfoComps[item.Key].ImgCurrency.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        /*_txtName.text = ItemSData.ItemName;
        _imgItem.sprite = ItemSData.ItemImage;
        foreach (var item in _dictCurrencyInfoComps)
        {
            if (!ItemSData.DictPriceInfo.ContainsKey(item.Key)) return;

            string ItemPrice = ItemSData.DictPriceInfo[item.Key].Price.ToString();
            Sprite ItemImage = ItemSData.DictPriceInfo[item.Key].Image;
            _dictCurrencyInfoComps[item.Key].TxtPrice.text = ItemPrice;
            _dictCurrencyInfoComps[item.Key].ImgCurrency.sprite = ItemImage;

            _dictCurrencyInfoComps[item.Key].TxtPrice.gameObject.SetActive(true);
            _dictCurrencyInfoComps[item.Key].ImgCurrency.gameObject.SetActive(true);
        }*/

        /*string filePath = Application.persistentDataPath + FRUITS_DATA_PATH;

        List<Fruits> list = new();
        Fruits a = new(EFruits.Apple, 5);
        Fruits b = new(EFruits.Banana, 3);

        list.Add(a);
        list.Add(b);
        FruitsIventory fi = new(list);
        JSONDataHelper.SaveToJSon<FruitsIventory>(fi, filePath);
        Debug.Log("save success");*/
    }

    public virtual void OnClick()
    {
        UIManager.Instance.TogglePopup(EPopup.ItemShopDetail, true);
        EventsManager.Instance.NotifyObservers(EEvents.ShopItemOnClick, this);
    }

    public virtual bool HandleBuyItem()
    {
        return false;
    }
}
