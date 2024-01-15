using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : GameObjectManager
{
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

    //Box type 1 thì có 5 loại gift, type 3 khó phá nhất thì cho HP
    [Header("Mystery Gift")] //Random 1 trong các gift sau - có thể thêm enemy nhỏ
    [SerializeField] private Transform _apple;
    [SerializeField] private Transform _banana;
    [SerializeField] private Transform _cherry;
    [SerializeField] private Transform _orange;
    [SerializeField] private Transform _mushroom;
    [SerializeField] private Transform _hp;

    //Có bug(?) khi nhảy lên box và bấm nhảy tiếp thì bật rất cao! @@
    //Chỉnh lại sound

    [Header("Sound")]
    [SerializeField] private AudioSource _brokeSound;
    [SerializeField] private AudioSource _gotHitSound;

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
        AnimationController();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.PLAYER_TAG) && !_isGotHit)
        {
            _healthPoint--;
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnJumpPassive, null);
            
            _isGotHit = true; //Mark this box has been hitted and make sure only applied force once
            if (_healthPoint == 0)
            {
                Invoke(nameof(AllowSpawnPiece), _delaySpawnPiece);
                _brokeSound.Play();
            }
            else
                _gotHitSound.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(GameConstants.BULLET_TAG))
        {
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.BulletOnHit, null);
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
            int randomGift = Random.Range(1, 6);
            switch (randomGift)
            {
                case 1:
                    Instantiate(_apple, transform.position, Quaternion.identity, null);
                    //Debug.Log("A");
                    break;
                case 2:
                    Instantiate(_banana, transform.position, Quaternion.identity, null);
                    //Debug.Log("B");
                    break;
                case 3:
                    Instantiate(_cherry, transform.position, Quaternion.identity, null);
                    //Debug.Log("C");
                    break;
                case 4:
                    Instantiate(_orange, transform.position, Quaternion.identity, null);
                    //Debug.Log("O");
                    break;

                case 5:
                    Instantiate(_mushroom, transform.position, Quaternion.identity, null);
                    //Debug.Log("M");
                    break;
            }
        }
        else if (type == 3)
            Instantiate(_hp, transform.position, Quaternion.identity, null);
    }


    private void SetBackIdle()
    {
        if (_healthPoint != 0)
        {
            _anim.SetTrigger("Idle");
        }
    }
}
