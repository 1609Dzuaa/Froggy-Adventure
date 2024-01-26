using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletInfor
{
    string _type;
    string _id;
    bool _isDirectionRight;
    Vector3 _shootPosition;

    public BulletInfor(string type, string id, bool isDirectionRight, Vector3 shootPosition)
    {
        _type = type;
        _id = id;
        _isDirectionRight = isDirectionRight;
        _shootPosition = shootPosition;
    }

    public string Type { get { return _type; } }

    public string ID { get { return _id; } }

    public bool IsDirectionRight { get { return _isDirectionRight; } }

    public Vector3 ShootPosition { get { return _shootPosition; } }
}

public class BulletController : MonoBehaviour
{
    [Header("Bullet SO")]
    [SerializeField] BulletStats _bulletStats;

    [Header("Pieces & Position")]
    [SerializeField] private GameObject _piece1;
    [SerializeField] private GameObject _piece2;
    [SerializeField] private Transform _piece1Position;
    [SerializeField] private Transform _piece2Position;

    [Header("Horizontal Or Vertical")]
    [SerializeField] private bool _isHorizontal;

    private Rigidbody2D _rb;
    private float _entryTime;
    private bool _isDirectionRight = false;
    private string _type;
    private string _bulletID;

    public string BulletID { get { return _bulletID; } set { _bulletID = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //Debug.Log("Id cua tao la: " + _bulletID);
    }

    private void OnEnable()
    {
        _entryTime = Time.time;
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.BulletOnReceiveInfo, ReceiveInfo);
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.BulletOnHit, DamageTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _entryTime >= _bulletStats.ExistTime)
            gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_isHorizontal)
        {
            if (_isDirectionRight)
                _rb.velocity = new Vector2(_bulletStats.BulletSpeed, 0f);
            else
                _rb.velocity = new Vector2(-_bulletStats.BulletSpeed, 0f);
        }
        else
            _rb.velocity = new Vector2(0, -_bulletStats.BulletSpeed);
    }

    private void OnDisable()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.BulletOnHit, DamageTarget);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.BulletOnReceiveInfo, ReceiveInfo);
        //Debug.Log("unsub event thanh cong");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConstants.PLAYER_TAG))
        {
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnTakeDamage, _isDirectionRight);
            SpawnBulletPieces();
            gameObject.SetActive(false);
        }
        else if (collision.collider.CompareTag(GameConstants.GROUND_TAG) || collision.collider.CompareTag(GameConstants.BULLET_TAG))
        {
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
        GameObject hitShieldEff = EffectPool.Instance.GetObjectInPool(GameEnums.EEfects.HitShield);
        hitShieldEff.SetActive(true);
        hitShieldEff.GetComponent<EffectController>().SetPosition(transform.position);
    }

    private void DamageTarget(object obj)
    {
        if ((string)obj != _bulletID)
            return;

        SpawnBulletPieces();
        gameObject.SetActive(false);
    }

    private void ReceiveInfo(object  obj)
    {
        //prob here
        BulletInfor bulletInfo = (BulletInfor)obj;
        if (_bulletID != bulletInfo.ID) 
            return;

        if (_isDirectionRight != bulletInfo.IsDirectionRight && bulletInfo.Type != GameConstants.BEE_BULLET)
            transform.Rotate(0, 180, 0);
        _isDirectionRight = bulletInfo.IsDirectionRight;
        _type = bulletInfo.Type;
        transform.position = bulletInfo.ShootPosition;
    }
}
