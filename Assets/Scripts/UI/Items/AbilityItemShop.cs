using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;
using static GameEnums;

public class AbilityItemShop : ItemShop
{
    [SerializeField] Image _imageFrame;
    [SerializeField] ESkills _skill;
    [HideInInspector] public SpecialItemStaticData SISData;
    bool _isBuyable;
    int _fruitLack = 0;

    protected override void Awake()
    {
        //Check xem cái ability item này đã đủ điều kiện mở khoá chưa:
        //Unlock = số lượng quả nào đấy quy định (5 quả apple, 8 quả kiwi, ...)
        //để mở khoá và có thể mua nó
        //VD: "Thu thập 5 trái Kiwis để có thể mua nó, hiện còn thiếu 2"
        base.Awake();

        SISData = (SpecialItemStaticData)ItemSData;
        if (SISData == null)
            Debug.Log("Null cmnr");
        string filePath = Application.dataPath + FRUITS_DATA_PATH;

        /*List<Fruits> list = new();
        Fruits a = new(EFruits.Apple, 5);
        Fruits b = new(EFruits.Banana, 3);

        list.Add(a);
        list.Add(b);
        FruitsIventory fi = new(list);
        JSONDataHelper.SaveToJSon<FruitsIventory>(fi, filePath);
        Debug.Log("save success");*/
        _fruitLack = SISData.Ability.FruitsRequired;
        FruitsIventory fI = JSONDataHelper.LoadFromJSon<FruitsIventory>(filePath);
        foreach (var fruit in fI.Fruits)
        {
            if (fruit.FruitName == SISData.Ability.FruitName)
            {
                if (fruit.FruitCount >= SISData.Ability.FruitsRequired)
                {
                    //bỏ lock item, cho phép mua
                    _imgItem.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    _imageFrame.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    _isBuyable = true;
                }
                else
                {
                    _fruitLack = SISData.Ability.FruitsRequired - fruit.FruitCount;
                    _imgItem.color = new Color(.45f, .45f, .45f, 1.0f);
                }
                break;
            }
        }

        //lock thông tin item
        if (!_isBuyable)
        {
            _txtName.text = "???";
            _dictCurrencyInfoComps[ECurrency.Silver].TxtPrice.text = "???";
            _dictCurrencyInfoComps[ECurrency.Gold].TxtPrice.text = "???";
        }
    }

    protected override void Start()
    {
        base.Start();

    }

    public override void OnClick()
    {
        if (_isBuyable)
            base.OnClick();
        else
        {
            string content = "Cannot Buy This Item, Collect " + _fruitLack + " More " + SISData.Ability.FruitName + " To Buy It!";
            NotificationParam param = new NotificationParam(content, true, false, null, null, null);
            ShowNotificationHelper.ShowNotification(param);
        }
    }

    public override bool HandleBuyItem()
    {
        _isPurchaseSuccess = UnlockItemHelper.HandleUnlockSkill(_skill);

        //mua thành công thì tắt ItemDetail mở thông báo
        if (_isPurchaseSuccess)
        {
            EventsManager.Instance.NotifyObservers(EEvents.OnUnlockSkill, ItemSData);
            UIManager.Instance.TogglePopup(EPopup.ItemShopDetail, false);
            UIManager.Instance.TogglePopup(EPopup.Ability, true);
        }

        return _isPurchaseSuccess;
    }
}
