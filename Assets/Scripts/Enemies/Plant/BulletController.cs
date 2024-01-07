using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float _bulletSpeed;

    [Header("Time")]
    [SerializeField] private float _existTime;

    [Header("Pieces & Position")]
    [SerializeField] private GameObject _piece1;
    [SerializeField] private GameObject _piece2;
    [SerializeField] private Transform _piece1Position;
    [SerializeField] private Transform _piece2Position;

    [Header("Horizontal Or Vertical")]
    [SerializeField] private bool _isHorizontal;

    [Header("Effect")]
    [SerializeField] private GameObject _hitShieldEffect;

    private Rigidbody2D _rb;
    private float _entryTime;
    private bool _isDirectionRight = false;
    private int _type;
    private int _id;

    public bool IsDirectionRight { set { _isDirectionRight = value; } }

    public int Type { set {  _type = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _id = GameConstants.BULLET_ID;
    }

    private void OnEnable()
    {
        _entryTime = Time.time;
        EventsManager.Instance.SubcribeAnEvent(GameEnums.EEvents.EnemiesOnDie, OnDamageAllies);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _entryTime >= _existTime)
            gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_isHorizontal)
        {
            if (_isDirectionRight)
                _rb.velocity = new Vector2(_bulletSpeed, 0f);
            else
                _rb.velocity = new Vector2(-_bulletSpeed, 0f);
        }
        else
            _rb.velocity = new Vector2(0, -_bulletSpeed);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnsubcribeAnEvent(GameEnums.EEvents.EnemiesOnDie, OnDamageAllies);
        //Debug.Log("unsub event thanh cong");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == GameConstants.PLAYER_NAME || collision.collider.CompareTag(GameConstants.GROUND_TAG))
        {
            if (collision.collider.name == GameConstants.PLAYER_NAME)
            {
                var playerScript = collision.collider.GetComponent<PlayerStateManager>();
                playerScript.IsHitFromRightSide = _isDirectionRight;
                EventsManager.Instance.InvokeAnEvent(GameEnums.EEvents.EnemiesOnDamagePlayer, null);
            }
            SpawnBulletPieces();
            gameObject.SetActive(false);
        }
        else if (collision.collider.CompareTag(GameConstants.SHIELD_TAG))
        {
            SpawnHitShieldEffect();
            SpawnBulletPieces();
            gameObject.SetActive(false);
        }
    }

    public void SpawnBulletPieces()
    {
        GameObject[] pieces = new GameObject[2];
        pieces[0] = BulletPiecePool.Instance.GetObjectInPool(_type).Pair1;
        pieces[1] = BulletPiecePool.Instance.GetObjectInPool(_type).Pair2;
        //Debug.Log("Type: " + _type);

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].SetActive(true);
            if (i == 0)
                pieces[i].GetComponent<BulletPieceController>().SpawnPosition = _piece1Position.position;
            else
                pieces[i].GetComponent<BulletPieceController>().SpawnPosition = _piece2Position.position;
            pieces[i].GetComponent<BulletPieceController>().IsShotFromRight = _isDirectionRight;
        }
    }

    private void SpawnHitShieldEffect()
    {
        GameObject hitShieldEff = EffectPool.Instance.GetObjectInPool(GameConstants.HIT_SHIELD_EFFECT);
        hitShieldEff.SetActive(true);
        hitShieldEff.GetComponent<EffectController>().SetPosition(transform.position);
    }

    private void OnDamageAllies(object obj)
    {
        if (_id != (int)obj)
            return;

        SpawnBulletPieces();
        gameObject.SetActive(false);
        Debug.Log("ID cua tao la " + (int)obj);
    }
}
