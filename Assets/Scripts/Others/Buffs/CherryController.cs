using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : ItemsController
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void ApplyBuff()
    {
        PlayerInvisibleBuff.Instance.ApplyBuff();
    }
}
