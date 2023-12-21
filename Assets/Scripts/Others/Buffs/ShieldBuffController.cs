using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuffController : ItemsController
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var playerManagerScript = collision.gameObject.GetComponent<PlayerStateManager>();
            playerManagerScript.PlayerShield.SetActive(true);
            playerManagerScript.GetCollectSound().Play();
            Instantiate(_collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(this.gameObject);
        }
        //base.OnTriggerEnter2D(collision);
    }
}
