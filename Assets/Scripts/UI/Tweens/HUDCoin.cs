using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static GameEnums;
using static GameConstants;

public class CoinInfo
{
    public ECurrency Currency;
    public int CoinGiven;

    public CoinInfo(ECurrency currency, int coinGiven)
    {
        Currency = currency;
        CoinGiven = coinGiven;
    }
}

public class HUDCoin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _txtSCoinCollected;
    [SerializeField] TextMeshProUGUI _txtGCoinCollected;
    [SerializeField] Image _imageSCoin;
    [SerializeField] Image _imageGCoin;

    [Header("Tween Related")]
    [SerializeField] float _duration;
    [SerializeField] Ease _ease;
    [HideInInspector] public int SCoinCollected, GCoinCollected = 0;
    bool _isDoubleReward;

    // Start is called before the first frame update
    void Start()
    {
        ResetCoins();
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnCollectCoin, CollectCoin);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnBeingCursed, DoubleReward);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnRewardCoin, CollectCoin);
    }

    private void RewardCoin(object obj)
    {
       
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnCollectCoin, CollectCoin);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnBeingCursed, DoubleReward);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnRewardCoin, CollectCoin);
    }

    public void ResetCoins()
    {
        SCoinCollected = GCoinCollected = DEFAULT_ITEM_COUNT;
        _txtSCoinCollected.text = _txtGCoinCollected.text = DEFAULT_ITEM_COUNT.ToString();
    }

    private void CollectCoin(object obj) 
    {
        CoinInfo info = obj as CoinInfo;
        Tween(info);
    }

    private void DoubleReward(object obj) => _isDoubleReward = true;

    private void Tween(CoinInfo info)
    {
        if (info.Currency == ECurrency.Silver)
        {
            int endValue = SCoinCollected + ((!_isDoubleReward) ? info.CoinGiven : info.CoinGiven * 2);
            DOTween.To(() => SCoinCollected, x => SCoinCollected = x, endValue, _duration)
                .OnUpdate(() =>
                {
                    _txtSCoinCollected.text = SCoinCollected.ToString();
                });
        }
        else
        {
            int endValue = GCoinCollected + ((!_isDoubleReward) ? info.CoinGiven : info.CoinGiven * 2);
            DOTween.To(() => GCoinCollected, x => GCoinCollected = x, endValue, _duration)
                .OnUpdate(() =>
                {
                    _txtGCoinCollected.text = GCoinCollected.ToString();
                });
        }
    }
}
