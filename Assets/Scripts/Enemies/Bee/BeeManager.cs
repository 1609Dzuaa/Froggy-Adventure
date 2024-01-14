using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MEnemiesManager
{
    //Xài prefab Bee thì chỉ định 3 phần min/max + Nest
    //Only Chase & Attack Player if he comes too close to the Nest
    [Header("Nest")]
    [SerializeField] private Transform _beeNest;

    [Header("Boundaries")]
    [SerializeField] private Transform _boundaryLeft;
    [SerializeField] private Transform _boundaryRight;

    [Header("Weapon Field")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPosition;

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

    public Transform BoundaryLeft { get { return _boundaryLeft; } }

    public Transform BoundaryRight { get { return _boundaryRight; } }

    public BeeIdleState GetBeeIdleState() { return _beeIdleState; }

    public BeePatrolState GetBeePatrolState() { return _beePatrolState; }

    public BeeChaseState GetBeeChaseState() { return _beeChaseState; }

    public BeeAttackState GetBeeAttackState() { return _beeAttackState; }

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
        MEnemiesGotHitState = _beeGotHitState;
        _playerCheck = FindObjectOfType<PlayerStateManager>().transform;
    }

    protected override void Update()
    {
        _state.Update();
        DetectedPlayer();
        //Debug.Log("IFR: " + _isFacingRight);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override bool DetectedPlayer()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return _hasDetectedPlayer = false;

        return _hasDetectedPlayer = Vector2.Distance(_playerCheck.position, _beeNest.position) <= _triggerAttackRange;
    }

    private void OnDrawGizmos()
    {
        Vector2 _attackPos = new Vector2(_playerCheck.position.x, _playerCheck.position.y + _adjustYAxisAttackRange);
        Gizmos.DrawSphere(_attackPos, 1.0f);
    }

    public void SpawnBullet()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
        {
            ChangeState(_beeIdleState);
            _mustAttack = false;
            return;
        }

        GameObject bullet = BulletPool.Instance.GetObjectInPool(GameConstants.BEE_BULLET);

        if (bullet != null)
        {
            bullet.SetActive(true);
            bullet.transform.position = _shootPosition.position;
            bullet.GetComponent<BulletController>().IsDirectionRight = _isFacingRight;
            bullet.GetComponent<BulletController>().Type = GameConstants.BEE_BULLET;
            //Debug.Log("I'm here");
        }
    }

    public void AllowUpdateAttackState()
    {
        _beeAttackState.SetAllowUpdate(true);
        //Delay việc update ở state shoot nếu 0 sẽ bị loạn state
    }
}
