using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryController : ItemsController
{
    protected override void ApplyBuff()
    {
        PlayerAbsorbBuff.Instance.ApplyBuff();
    }
}
