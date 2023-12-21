using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    [SerializeField] protected Transform _collectedEffect;

    //Jump higher when eat
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var playerManagerScript = collision.gameObject.GetComponent<PlayerStateManager>();
            playerManagerScript.IncreaseOrangeCount();
            playerManagerScript.GetCollectSound().Play();
            Instantiate(_collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(this.gameObject);
        }
    }
}
