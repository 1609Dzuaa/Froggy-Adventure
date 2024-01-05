using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameConstants.PLAYER_NAME)
        {
            ApplyBuff();
            SoundsManager.Instance.GetTypeOfSound(GameConstants.COLLECT_FRUITS_SOUND).Play();
            SpawnEffect();
            Destroy(gameObject);
        }
    }

    protected virtual void ApplyBuff()
    {
        //Each item will apply different buff in here
    }

    protected virtual void SpawnEffect()
    {
        GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameConstants.COLLECT_FRUITS_EFFECT);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
