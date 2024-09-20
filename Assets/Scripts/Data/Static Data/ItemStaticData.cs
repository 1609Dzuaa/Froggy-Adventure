using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/ItemStaticData")]
public class ItemStaticData : ScriptableObject
{
    public string ItemName;
    public List<PriceInfo> ListPriceInfos;
    public Sprite ItemImage;
    [Header("Mô tả khi nhấn vào")] public string ItemDescribe;

    public Dictionary<ECurrency, PriceInfo> DictPriceInfo = new();

    private void OnEnable()
    {
        foreach (var item in ListPriceInfos)
        {
            if (!DictPriceInfo.ContainsKey(item.Currency))
                DictPriceInfo.Add(item.Currency, item);
        }
    }
}

[System.Serializable]
public class PriceInfo
{
    public int Price;
    public Sprite Image;
    public ECurrency Currency;
}
