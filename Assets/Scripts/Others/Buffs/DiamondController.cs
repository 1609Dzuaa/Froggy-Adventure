using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : ItemsController
{
    protected override void SpawnEffect()
    {
        GameObject collectEff = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.CollectDiamond);
        collectEff.SetActive(true);
        collectEff.transform.position = transform.position;
    }
}
