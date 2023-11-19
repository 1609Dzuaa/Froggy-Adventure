using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float bulletSpeed;

    [Header("Time")]
    [SerializeField] private float existTime;

    private Rigidbody2D rb;
    private float entryTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        entryTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - entryTime >= existTime)
        {
            //Spawn lá hoặc effect gì đấy
            Destroy(this.gameObject);
        }
        else
            rb.velocity = new Vector2(bulletSpeed, 0f); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Player")
        {
            //Spawn lá hoặc effect gì đấy
            Destroy(this.gameObject);
        }
    }
}
