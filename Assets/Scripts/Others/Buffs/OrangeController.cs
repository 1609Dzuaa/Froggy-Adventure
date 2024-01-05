using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeController : ItemsController
{
    protected override void ApplyBuff()
    {
        PlayerSpeedBuff.Instance.ApplyBuff();
    }
}
