using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnums;

//mua thành công thì fire close popup
public struct BuyStruct
{
    public ItemShop Item;
    public Action CloseCallback;

    public BuyStruct(ItemShop item, Action close)
    {
        Item = item;
        CloseCallback = close;
    }
}

public class ItemShopDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _txtName;
    [SerializeField] Image _imgItem;
    [SerializeField] TextMeshProUGUI _txtDescribe;
    [SerializeField] PopupController _popupItemDetail;
    ItemShop _itemShop;

    // Start is called before the first frame update
    void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.ShopItemOnClick, ShowDetail);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.ShopItemOnClick, ShowDetail);
    }

    private void ShowDetail(object obj)
    {
        _itemShop = (ItemShop)obj;
        _txtName.text = _itemShop.ItemSData.ItemName;
        _imgItem.sprite = _itemShop.ItemSData.ItemImage;
        _txtDescribe.text = _itemShop.ItemSData.ItemDescribe;
    }

    public void ButtonBuyOnClick()
    {
        BuyStruct buyStruct = new BuyStruct(_itemShop, _popupItemDetail.OnClose);
        EventsManager.Instance.NotifyObservers(EEvents.PlayerOnBuyShopItem, buyStruct);
    }
}
