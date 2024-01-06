using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeController : ItemsController
{
    protected override void ApplyBuff()
    {
        //Cho phép Update(chạy) buff mà thằng này giữ
        BuffsManager.Instance.GetTypeOfBuff(_buff).ApplyBuff();
        Debug.Log("buff r");
    }
}
