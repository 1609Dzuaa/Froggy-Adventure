using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : ItemsController
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void ApplyBuff()
    {
        PlayerJumpBuff.Instance.ApplyBuff();
    }
}
