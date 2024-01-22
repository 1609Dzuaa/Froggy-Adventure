using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPiecesController : MonoBehaviour
{
    [SerializeField] private Vector2 _bounceForce;
    [SerializeField] private float _existTime;

    private Rigidbody2D _rb;
    private float _entryTime;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(_bounceForce);
        _entryTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _entryTime >= _existTime)
            Destroy(gameObject);
    }
}
