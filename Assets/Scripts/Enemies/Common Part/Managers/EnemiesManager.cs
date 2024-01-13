using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemiesManager : CharactersManager
{
    [Header("Player Check")]
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected float _checkDistance;
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected Vector2 _knockForce; //Knock Force khi bị hit
    [SerializeField] protected float _attackDelay;

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] protected float degreeEachRotation;
    [SerializeField] protected float timeEachRotate;

    protected bool _hasDetectedPlayer;
    protected bool _hasGotHit; //Đánh dấu bị Hit, tránh Trigger nhiều lần
    protected Collider2D _collider2D;
    protected SpriteRenderer _spriteRenderer;
        
    //Public Field
    public Vector2 KnockForce { get { return _knockForce; } }

    public float GetAttackDelay() { return _attackDelay; }

    public float GetTimeEachRotate() { return timeEachRotate; }

    public float GetDegreeEachRotation() { return degreeEachRotation; }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public Collider2D GetCollider2D { get { return _collider2D; } set { _collider2D = value; } }

    public SpriteRenderer GetSpriteRenderer { get => _spriteRenderer; }

    protected override void Awake()
    {
        base.Awake(); //Lấy các ref components trong đây
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable() { }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (transform.rotation.eulerAngles.y == 180f)
            _isFacingRight = true;
        //Debug.Log("IfR: " + _isFacingRight);

    }

    protected override void Update()
    {
        base.Update();
        DetectedPlayer(); //Enemies nào thì cũng phải DetectPlayer, cho vào đây là hợp lý
        DrawRayDetectPlayer();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConstants.PLAYER_TAG))
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnTakeDamage, _isFacingRight);
    }

    protected virtual bool DetectedPlayer()
    {
        //Cân nhắc có nên detect luôn cái layer Ignore 0 ?

        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return _hasDetectedPlayer = false;
        
        if (!_isFacingRight)
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _checkDistance, _playerLayer);
        else
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _checkDistance, _playerLayer);

        return _hasDetectedPlayer;
    }

    protected virtual void DrawRayDetectPlayer()
    {
        if (_hasDetectedPlayer)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.left * _checkDistance, Color.red);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.right * _checkDistance, Color.red);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.left * _checkDistance, Color.green);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.right * _checkDistance, Color.green);
        }
    }

    protected void DestroyItSelf()
    {
        Destroy(gameObject);
    }

}
