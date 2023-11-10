using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningScript : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float verticalSpeed = 10.0f;

    [Header("Distance")]
    [SerializeField] private float distanceTravel = 5.0f;
    private Rigidbody2D rb;
    private Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > startPos.y + distanceTravel)
            Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0f, verticalSpeed, 0f) * Time.deltaTime;
    }
}
