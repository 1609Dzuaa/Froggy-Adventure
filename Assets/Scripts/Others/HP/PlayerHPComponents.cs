using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameConstants;
using static GameEnums;

public class PlayerHPComponents : MonoBehaviour
{
    [SerializeField] Transform _top, _bot;
    [SerializeField] float _duration;
    bool _isUp = true;
    float _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = (_isUp) ? _top.localPosition.y : _bot.localPosition.y;
        transform.DOLocalMoveY(_target, _duration).OnComplete(() =>
        {
            _isUp = !_isUp;
            Start();
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            if (PlayerHealthManager.Instance.CurrentHP < PlayerHealthManager.Instance.MaxHP)
            {
                EventsManager.NotifyObservers(EEvents.OnChangeHP, EHPStatus.AddOneHP);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
