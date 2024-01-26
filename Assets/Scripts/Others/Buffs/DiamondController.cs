using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : ItemsController
{
    protected override void SpawnEffect()
    {
        GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameEnums.EEfects.CollectDiamond);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
