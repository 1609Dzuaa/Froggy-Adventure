using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPieceController : MonoBehaviour
{
    [Header("Bouncing Force")]
    [SerializeField] private Vector2 bouncingForce;

    [Header("Exist Time")]
    [SerializeField] private float existTime;

    private Rigidbody2D rb;
    private float entryTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(bouncingForce);
        entryTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - entryTime >= existTime) 
        {
            Destroy(this.gameObject);
        }
    }
}
