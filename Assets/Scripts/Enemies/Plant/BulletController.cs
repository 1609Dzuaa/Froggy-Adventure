using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float bulletSpeed;

    [Header("Time")]
    [SerializeField] private float existTime;

    [Header("Pieces & Position")]
    [SerializeField] private Transform piece1;
    [SerializeField] private Transform piece2;
    [SerializeField] private Transform piece1Position;
    [SerializeField] private Transform piece2Position;

    private Rigidbody2D rb;
    private float entryTime;
    private bool isDirectionRight = false;

    public void SetIsDirectionRight(bool para) { this.isDirectionRight = para; }
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
    }

    private void FixedUpdate()
    {
        if (isDirectionRight)
            rb.velocity = new Vector2(bulletSpeed, 0f);
        else
            rb.velocity = new Vector2(-bulletSpeed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Có nên thử cho nó damage allies của mình || box ?
        if(collision.collider.name == "Player" || collision.collider.CompareTag("Ground"))
        {
            SpawnBulletPieces();
            Destroy(this.gameObject);
        }
    }

    private void SpawnBulletPieces()
    {
        Instantiate(piece1, piece1Position.position, Quaternion.identity, null);
        Instantiate(piece2, piece2Position.position, Quaternion.identity, null);
    }
}
