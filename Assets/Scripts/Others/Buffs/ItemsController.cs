using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    [SerializeField] protected Transform _collectedEffect;

    //Jump higher when eat
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            ApplyBuff();
            //Sound
            Instantiate(_collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
        }
    }

    protected virtual void ApplyBuff()
    {
        //Each item will apply different buff in here
    }
}
