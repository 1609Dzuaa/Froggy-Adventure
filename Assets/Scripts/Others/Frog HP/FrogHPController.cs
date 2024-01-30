using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHPController : ItemsController
{
    [Header("Parent")]
    [SerializeField] private GameObject _parent;

    [Header("Boundaries")]
    [SerializeField] private Transform _topPos;
    [SerializeField] private Transform _botPos;

    [Header("Speed")]
    [SerializeField] private float _speedY;

    [Header("Collected Effect")]
    [SerializeField] private Transform _collectedEffect;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= _topPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, _topPos.position.y, transform.position.z);
            _speedY = -_speedY;
        }
        else if (transform.position.y <= _botPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, _botPos.position.y, transform.position.z);
            _speedY = -_speedY;
        }
        transform.position += new Vector3(0f, _speedY, 0f) * Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            SpawnEffect();
            PlayerHealthManager.Instance.ChangeHPState(GameConstants.HP_STATE_NORMAL);
            SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.CollectHPSfx, 1.0f);
            Destroy(_parent);
        }
    }

    protected override void SpawnEffect()
    {
        GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameEnums.EEfects.CollectHP);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
