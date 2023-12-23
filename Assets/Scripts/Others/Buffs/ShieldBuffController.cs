using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuffController : ItemsController
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            PlayerShieldBuff.Instance.ApplyBuff();
            Instantiate(_collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
        }
    }
}
