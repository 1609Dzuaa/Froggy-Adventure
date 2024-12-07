using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameConstants;

public class PlayerHPComponents : MonoBehaviour
{
    [SerializeField] Transform _top, _bot;
    [SerializeField] float _duration, _distance;
    float _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = transform.position.y + _distance;
        transform.DOMoveY(_target, _duration).OnComplete(() =>
        {
            _target = transform.position.y - 2 * _distance;
        }).SetLoops((int)LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            if (PlayerHealthManager.Instance.CurrentHP < PlayerHealthManager.Instance.MaxHP)
            {
                PlayerHealthManager.Instance.ChangeHPState(HP_STATE_NORMAL);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
