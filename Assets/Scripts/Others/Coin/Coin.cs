using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;
using DG.Tweening;

public class Coin : GameObjectManager
{
    [SerializeField] CoinData _data;
    [SerializeField] ECurrency _type;
    [SerializeField] Transform _playerRef;
    int _valueGiven = 0;
    bool _receivedNotify;
    Vector3 _target;

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _valueGiven = Random.Range(_data.MinValue, _data.MaxValue + 1);
        EventsManager.SubcribeToAnEvent(EEvents.OnMagnetizeCoins, MoveTowardPlayer);
    }

    private void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnMagnetizeCoins, MoveTowardPlayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            CoinInfo info = new(_type, _valueGiven);
            EventsManager.NotifyObservers(EEvents.OnCollectCoin, info);

            MarkAsDeleted();
            Destroy(gameObject);
        }
    }

    public void MoveTowardPlayer(object obj = null)
    {
        //_target = (Vector3)obj;
        if (!_receivedNotify)
        {
            _receivedNotify = true;
            DOTween.To(() => transform.position, x => transform.position = x, _playerRef.position/*_playerRef.position*/, _data.TweenDuration)
               .SetEase(Ease.Linear)
               .OnUpdate(() =>
               {
                   //Debug.Log("target: " + _playerRef.position);
                   //if (Vector3.Distance(transform.position, target) < NEAR_ZERO_THRESHOLD)
                   //{
                   //DOTween.Kill(transform.position);
                   //}
               }).OnComplete
               (() => 
               { 
                   DOTween.Kill(transform);
                   CoinInfo info = new(_type, _valueGiven);
                   EventsManager.NotifyObservers(EEvents.OnCollectCoin, info);
                   Destroy(gameObject);
               });
               //.SetLoops(-1, LoopType.Restart);
            //Debug.Log("received");
        }
    }
}
