using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MEnemiesManager
{
    //Only Chase & Attack Player if he comes too close to the Nest
    [Header("Nest")]
    [SerializeField] private Transform _beeNest;

    [Header("Weapon Field")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPos;

    [Header("Range")]
    [SerializeField] private float _triggerAttackRange;
    [SerializeField] private float _adjustYAxisAttackRange;

    //Đổi Enemies speed thành kiểu Vector2 hết, chỗ nào cần xài thì xài, 0 cần thì Rb.velo.y giữ nguyên
    [Header("YOffset")]
    [SerializeField] private float _yOffset;

    private BeeIdleState _beeIdleState = new();
    private BeePatrolState _beePatrolState = new();
    private BeeChaseState _beeChaseState = new();
    private BeeAttackState _beeAttackState = new();
    private BeeGotHitState _beeGotHitState = new();
    private bool _mustAttack;

    public float AdJustYAxisAttackRange() { return _adjustYAxisAttackRange; }

    public float YOffSet { get { return _yOffset; } }

    public BeeIdleState GetBeeIdleState() { return _beeIdleState; }

    public BeePatrolState GetBeePatrolState() { return _beePatrolState; }

    public BeeChaseState GetBeeChaseState() { return _beeChaseState; }

    public BeeAttackState GetBeeAttackState() { return _beeAttackState; }

    public BeeGotHitState GetBeeGotHitState() { return _beeGotHitState; }

    public Transform GetPlayer() { return _playerCheck; }

    public bool MustAttack { get { return _mustAttack; } set { _mustAttack = value; } }

    public void SetIsFacingRight(bool para) { _isFacingRight = para; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        _state = _beeIdleState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        _state.Update();
        DetectedPlayer();
        Debug.Log("IFR: " + _isFacingRight);
        /*if (_hasDetectedPlayer)
            Debug.Log("Too close");*/
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override bool DetectedPlayer()
    {
        //Viết như này thì nhanh hơn
        //Nhớ gọi func :v
        return _hasDetectedPlayer = Vector2.Distance(_playerCheck.position, _beeNest.position) <= _triggerAttackRange;
        /*if (Vector2.Distance(_playerCheck.position, _beeNest.position) <= _triggerAttackRange)
            return true;
        return false;*/
    }

    private void OnDrawGizmos()
    {
        Vector2 _attackPos = new Vector2(_playerCheck.position.x, _playerCheck.position.y + _adjustYAxisAttackRange);
        Gizmos.DrawSphere(_attackPos, 1.0f);
    }

    public void SpawnBullet()
    {
        Instantiate(_bullet, transform.position, Quaternion.identity);
    }

    public void AllowUpdateAttackState()
    {
        _beeAttackState.SetAllowUpdate(true);
    }
}
