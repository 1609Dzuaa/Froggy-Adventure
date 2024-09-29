using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public class Coin : GameObjectManager
{
    [SerializeField] CoinData _data;
    [SerializeField] ECurrency _type;
    int _valueGiven = 0;

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _valueGiven = Random.Range(_data.MinValue, _data.MaxValue + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            CoinInfo info = new(_type, _valueGiven);
            EventsManager.Instance.NotifyObservers(EEvents.OnCollectCoin, info);
            gameObject.SetActive(false);
        }
    }
}
