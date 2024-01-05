using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : ItemsController
{
    protected override void ApplyBuff()
    {
        PlayerJumpBuff.Instance.ApplyBuff();
    }
}
