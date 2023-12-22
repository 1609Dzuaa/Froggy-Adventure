using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeController : MonoBehaviour
{
    [SerializeField] private Transform collectedEffect;

    //Giảm tải bớt khối lượng dòng code cho class PlayerManager
    //Run faster when eat

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            JumpBuff.Instance.ApplyBuff();
            //SpeedBuff.Instance.ApplyBuff();
            Instantiate(collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
        }
    }
}
