using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[System.Serializable]
public class FruitLimited
{
    public EFruits Name;
    public Transform Fruit;
}

public class BoxController : GameObjectManager
{
    //Remake lại box:
    //Type 1 cho tiền
    //Type 2 cho hoa quả
    //Type 3 cho HP

    [Header("Type")]
    [Range(1,3)]
    [SerializeField] private int _boxType;

    [Header("Break Piece")]
    [SerializeField] private Transform _brPiece1;
    [SerializeField] private Transform _brPiece2;
    [SerializeField] private Transform _brPiece3;
    [SerializeField] private Transform _brPiece4;

    [Header("Pieces Position")]
    [SerializeField] private Transform _pos1;
    [SerializeField] private Transform _pos2;
    [SerializeField] private Transform _pos3;
    [SerializeField] private Transform _pos4;

    [Header("Gifts, Type 1 thì chỉ cần 2 field s, gCoin, type 2 cần 3 field fruits, type 3 cần HP")]
    [SerializeField] private Transform _sCoin;
    [SerializeField] private Transform _gCoin;

    [Header("Mỗi level chỉ giới hạn 3 loại fruit có thể lụm")]
    [SerializeField] private FruitLimited[] _arrFruitAllow;

    [Header("HP")]
    [SerializeField] private Transform _hp;

    [Header("Time")]
    [SerializeField] private float _delaySpawnPiece;

    private bool _isGotHit = false;
    private bool _allowSpawnPiece = false;
    private bool _hasSpawnPiece = false;
    private int _healthPoint;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _healthPoint = _boxType;
    }

    // Update is called once per frame
    void Update()
    {
        //bỏ update đi, dùng invoke cho nó
        AnimationController();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.PLAYER_TAG) && !_isGotHit)
        {
            _healthPoint--;
            EventsManager.NotifyObservers(EEvents.PlayerOnJumpPassive);
            
            _isGotHit = true; //Mark this box has been hitted and make sure only applied force once
            if (_healthPoint == 0)
            {
                Invoke(nameof(AllowSpawnPiece), _delaySpawnPiece);
                SoundsManager.Instance.PlaySfx(ESoundName.BoxBrokeSfx, 1.0f);
                //PlayerPrefs.SetString(ESpecialStates.Deleted + _ID, "deleted");
            }
            else
                SoundsManager.Instance.PlaySfx(ESoundName.BoxGotHitSfx, 1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(GameConstants.BULLET_TAG))
        {
            EventsManager.NotifyObservers(EEvents.BulletOnHit);
            _isGotHit = true;
            Invoke(nameof(AllowSpawnPiece), _delaySpawnPiece);
        }
    }

    private void AnimationController()
    {
        if (_isGotHit)
        {
            _anim.SetTrigger("GotHit");
            _isGotHit = false;
        }
        if (_allowSpawnPiece && !_hasSpawnPiece)
        {
            SpawnPiece();
            SpawnGift(_boxType);
            Destroy(gameObject);
        }
    }

    private void AllowSpawnPiece()
    {
        _allowSpawnPiece = true;
        //Event của animation GotHit
    }

    private void SpawnPiece()
    {
        _hasSpawnPiece = true;

        //Object pool cho thang nay luon?
        Instantiate(_brPiece1, _pos1.position, Quaternion.identity, null);
        Instantiate(_brPiece2, _pos2.position, Quaternion.identity, null);
        Instantiate(_brPiece3, _pos3.position, Quaternion.identity, null);
        Instantiate(_brPiece4, _pos4.position, Quaternion.identity, null);
        //Chú ý tham số cuối của hàm này, pass null nếu 0 muốn piece nhận thằng box làm parent :D
    }

    private void SpawnGift(int type)
    {
        if (type == 1)
        {
            int randomGift = Random.Range(0, 2);
            switch (randomGift)
            {
                case 0:
                    Instantiate(_sCoin, transform.position, Quaternion.identity, null);
                    break;
                case 1:
                    Instantiate(_gCoin, transform.position, Quaternion.identity, null);
                    break;
            }
        }
        else if (type == 3)
            Instantiate(_hp, transform.position, Quaternion.identity, null);
        else
        {
            int randomGift = Random.Range(0, 3);
            Instantiate(_arrFruitAllow[randomGift].Fruit, transform.position, Quaternion.identity, null);
        }
    }

    private void SetBackIdle()
    {
        if (_healthPoint != 0)
        {
            _anim.SetTrigger("Idle");
        }
    }
}
