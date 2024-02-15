using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPieceController : MonoBehaviour
{
    [Header("Bouncing Force")]
    [SerializeField] private Vector2 _bouncingForce;

    [Header("Exist Time")]
    [SerializeField] private float _existTime;

    [Header("Type & Ammount")]
    [SerializeField] private GameEnums.EPoolable _pieceType;

    private Rigidbody2D _rb;
    private float _entryTime;
    private bool _isShotFromRight = false; //Bắn từ bên nào để áp dụng vector lực hướng ngược lại

    public bool IsShotFromRight { set { _isShotFromRight = value; } }

    public Vector3 SpawnPosition { set { transform.position = value; } }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _entryTime = Time.time;
        _rb.AddForce((_isShotFromRight) ? _bouncingForce : _bouncingForce * new Vector2(-1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - _entryTime >= _existTime) 
            gameObject.SetActive(false);
    }
}
