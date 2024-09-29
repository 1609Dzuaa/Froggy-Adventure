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
        base.Awake();

        SISData = (SpecialItemStaticData)ItemSData;
        if (SISData == null)
            Debug.Log("Null cmnr");
        HandleDisplayItem();
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnItemEligibleCheck, HandleDisplayItem);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnItemEligibleCheck, HandleDisplayItem);
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
        _isPurchaseSuccess = ToggleAbilityItemHelper.ToggleLockSkill(_skill, false);

        //mua thành công thì tắt ItemDetail mở thông báo
        if (_isPurchaseSuccess)
        {
            EventsManager.Instance.NotifyObservers(EEvents.OnUnlockSkill, ItemSData);
            UIManager.Instance.TogglePopup(EPopup.ItemShopDetail, false);
            UIManager.Instance.TogglePopup(EPopup.Ability, true);
        }

        return _isPurchaseSuccess;
    }

    private void HandleDisplayItem(object obj = null)
    {
        string filePath = Application.dataPath + FRUITS_DATA_PATH;
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

        //xử lý lock thông tin item
        _txtName.text = (!_isBuyable) ? "???" : SISData.ItemName;
        _dictCurrencyInfoComps[ECurrency.Silver].TxtPrice.text = (!_isBuyable) ? "???" : SISData.DictPriceInfo[ECurrency.Silver].Price.ToString();
        _dictCurrencyInfoComps[ECurrency.Gold].TxtPrice.text = (!_isBuyable) ? "???" : SISData.DictPriceInfo[ECurrency.Gold].Price.ToString();

        //Debug.Log("callback called");
    }
}
