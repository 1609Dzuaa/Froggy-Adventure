using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaController : MonoBehaviour
{
    [SerializeField] private Transform collectedEffect;

    //Untouchable for some second when eat ?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var playerManagerScript = collision.gameObject.GetComponent<PlayerStateManager>();
            playerManagerScript.GetCollectSound().Play();
            Instantiate(collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(this.gameObject);
        }
    }
}
