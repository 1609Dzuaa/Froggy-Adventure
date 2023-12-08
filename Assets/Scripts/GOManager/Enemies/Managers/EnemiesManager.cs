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
    protected bool _hasDetectedPlayer;
    protected bool _hasGotHit; //Đánh dấu bị Hit, tránh Trigger nhiều lần
    protected Collider2D _collider2D;

    //Public Field
    public Vector2 KnockForce { get { return _knockForce; } }

    public float GetAttackDelay() { return this._attackDelay; }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public Collider2D Collider2D { get { return _collider2D; } }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //Lấy anim và rb từ CharactersManager
        _collider2D = GetComponent<Collider2D>();
        if (transform.rotation.eulerAngles.y == 180f)
            _isFacingRight = true;
        //Debug.Log("IfR: " + _isFacingRight);
    }

    protected override void Update()
    {
        base.Update();
        DetectPlayer(); //Enemies nào thì cũng phải DetectPlayer, cho vào đây là hợp lý
    }

    protected virtual void DetectPlayer()
    {
        if (!_isFacingRight)
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _checkDistance, _playerLayer);
        else
            _hasDetectedPlayer = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _checkDistance, _playerLayer);

        DrawRayDetectPlayer();
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
        Destroy(this.gameObject);
    }

}
