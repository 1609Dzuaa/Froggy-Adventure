using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuffController : ItemsController
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            PlayerShieldBuff.Instance.ApplyBuff();
            GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameConstants.COLLECT_FRUITS_EFFECT);
            collectEff.SetActive(true);
            collectEff.GetComponent<EffectController>().SetPosition(transform.position);
            Destroy(gameObject);
        }
    }
}
