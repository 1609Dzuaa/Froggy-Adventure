using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigManager : MEnemiesManager
{
    //Đôi lúc việc Player nhảy lên bên mạn trái/phải enemies dẫn đến việc Add 1 lượng Force rất lớn ?

    [Header("HP")]
    [SerializeField] private int _hp;

    [Header("Time")]
    [SerializeField] private float _flipDelay;
    [SerializeField] private float _destroyParentDelay;

    [Header("Player Reference")]
    [SerializeField] private Transform _playerRef;

    [Header("Chase Speed Red Form")]
    [SerializeField] private float _chaseSpeedRed;

    //private PigAttackGreenState _pigAtkGreenState = new();
    private PigAttackRedState _pigAtkRedState = new();
    private PigGotHitGreenState _pigGotHitGreenState = new();
    private PigGotHitRedState _pigGotHitRedState = new();

    public Transform PlayerRef { get { return _playerRef; } }

    public PigAttackRedState GetPigAttackRedState() { return _pigAtkRedState; }

    public bool HasGotHit { set { _hasGotHit = value; } }

    public int HP { get { return _hp; } }

    public float ChaseSpeedRedForm { get { return _chaseSpeedRed; } }

    private bool _isTurnRed;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasGotHit)
        {
            _hasGotHit = true;
            _hp--;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.SetCanDbJump(true); //Nhảy lên đầu Enemies thì cho phép DbJump tiếp
            playerScript.GetRigidBody2D().AddForce(playerScript.GetJumpOnEnemiesForce(), ForceMode2D.Impulse);
            
            if(!_isTurnRed)
            {
                _isTurnRed = true;
                ChangeState(_pigGotHitGreenState);
            }
            else
                ChangeState(_pigGotHitRedState);
        }
    }

    private void AllowUpdateGotHitGreen()
    {
        _pigGotHitGreenState.AllowUpdate = true;
        //Event của animation GotHit Green
    }

    private void AllowUpdateGotHitRed()
    {
        _pigGotHitRedState.AllowUpdate = true;
        //Event của animation GotHit Red
    }

    public IEnumerator DeleteParent()
    {
        yield return new WaitForSeconds(_destroyParentDelay);

        DestroyParent();
    }

    public void FlipLeft()
    {
        _isFacingRight = false;
        transform.Rotate(0, 180, 0);
    }

    public void FlipRight()
    {
        _isFacingRight = true;
        transform.Rotate(0, 180, 0);
    }

    public IEnumerator AllowFlip()
    {
        yield return new WaitForSeconds(_flipDelay);

        _pigAtkRedState.AllowFlip = true;
    }
}
