using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : ItemsController
{
    protected override void SpawnEffect()
    {
        GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameConstants.COLLECT_DIAMOND_EFFECT);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
