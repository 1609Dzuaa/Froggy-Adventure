using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapController : GameObjectManager
{
    [Header("Time")]
    [SerializeField] private float _delayFireOn;

    [Header("Fire")]
    [SerializeField] private GameObject _fire;

    private bool _hasGotHit;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit)
        {
            _hasGotHit = true;
            _anim.SetTrigger(GameConstants.FIRE_TRAP_ANIM_GOT_HIT);
            StartCoroutine(FireOn());
        }
    }

    private IEnumerator FireOn()
    {
        yield return new WaitForSeconds(_delayFireOn);

        _fire.SetActive(true);
        _anim.SetTrigger(GameConstants.FIRE_TRAP_ANIM_ON);
    }
}
