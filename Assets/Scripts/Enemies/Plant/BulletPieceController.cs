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
    private bool isShotFromRight = false; //Bắn từ bên nào để áp dụng vector lực hướng ngược lại
    
    public void SetIsShotFromRight(bool para) { this.isShotFromRight = para; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isShotFromRight)
            rb.AddForce(bouncingForce * new Vector2(-1f, 1f));
        else
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
