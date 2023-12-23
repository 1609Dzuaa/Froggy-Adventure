using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeController : ItemsController
{
    //Giảm tải bớt khối lượng dòng code cho class PlayerManager
    //Run faster when eat

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void ApplyBuff()
    {
        PlayerSpeedBuff.Instance.ApplyBuff();
    }
}
