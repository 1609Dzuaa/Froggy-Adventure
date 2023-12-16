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
    private bool _isFireOn;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (_isFireOn)
            _fire.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            _anim.SetTrigger("GotHit");
            StartCoroutine(FireOn());
        }
    }

    private IEnumerator FireOn()
    {
        yield return new WaitForSeconds(_delayFireOn);

        _isFireOn = true;
        _anim.SetTrigger("On");
    }
}
