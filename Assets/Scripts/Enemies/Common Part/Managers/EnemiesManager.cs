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

    //Public Field
    public Vector2 KnockForce { get { return _knockForce; } }

    public float GetAttackDelay() { return this._attackDelay; }

    public float GetTimeEachRotate() { return this.timeEachRotate; }

    public float GetDegreeEachRotation() { return this.degreeEachRotation; }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public Collider2D Collider2D { get { return _collider2D; } set { _collider2D = value; } }

    protected override void Awake()
    {
        base.Awake(); //Lấy anim và rb từ CharactersManager
        _collider2D = GetComponent<Collider2D>();
        //Nên vứt hết các khởi tạo trong Awake tránh 1 số TH bị Null Ref khi để trong Start
        //Vì Awake chạy trước Start và luôn chạy kể cả khi script bị disabled
    }

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
        if (collision.collider.name == "Player")
        {
            var playerScript = collision.collider.GetComponent<PlayerStateManager>();
            playerScript.ChangeState(playerScript.gotHitState);
            if (_isFacingRight)
                playerScript.GetRigidBody2D().AddForce(new Vector2(playerScript.GetPlayerStats.KnockBackForce.x, 0f));
            else
                playerScript.GetRigidBody2D().AddForce(new Vector2(-playerScript.GetPlayerStats.KnockBackForce.x, 0f));
        }
    }

    protected virtual bool DetectedPlayer()
    {
        if (PlayerInvisibleBuff.Instance.IsAllowToUpdate)
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
        Destroy(this.gameObject);
    }

}
