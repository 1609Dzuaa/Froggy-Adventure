using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : ItemsController
{
    protected override void ApplyBuff()
    {
        PlayerInvisibleBuff.Instance.ApplyBuff();
    }
}
