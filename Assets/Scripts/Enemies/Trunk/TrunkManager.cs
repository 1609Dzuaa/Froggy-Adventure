using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkManager : MEnemiesManager
{
    //Nghiên cứu object pool tối ưu cho các enemy spawn đạn

    [Header("Weapon Field")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPosition;

    [Header("Withdrawn Field")]
    [SerializeField] private float _withdrawnCheckDistance; //khcach withdrawn phải < khcach playerCheck (Obviously!)
    [SerializeField] Vector2 _withdrawnForce;

    private TrunkIdleState _trunkIdleState = new();
    private TrunkPatrolState _trunkPatrolState = new();
    private TrunkWithdrawnState _trunkRetreatState = new();
    private TrunkAttackState _trunkAttackState = new();
    private TrunkGotHitState _trunkGotHitState = new();

    private bool _canWithDrawn;

    public TrunkIdleState GetTrunkIdleState() { return _trunkIdleState; }

    public TrunkPatrolState GetTrunkPatrolState() { return _trunkPatrolState; }

    public TrunkWithdrawnState GetTrunkWithState() { return _trunkRetreatState; }

    public TrunkAttackState GetTrunkAttackState() { return _trunkAttackState; }

    public bool CanWithDrawn { get {  return _canWithDrawn; } }

    public Vector2 WithdrawnForce { get { return _withdrawnForce; } }

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
        _mEnemiesIdleState = _trunkIdleState;
        _mEnemiesGotHitState = _trunkGotHitState;
        base.SetUpProperties();
        //Debug.Log("SetProp");
    }

    protected override void Update()
    {
        WithDrawnCheck();
        base.Update();
        //Debug.Log("here");
    }

    private bool WithDrawnCheck()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return _canWithDrawn = false;

        if (!_isFacingRight)
            _canWithDrawn = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.left, _withdrawnCheckDistance, _enemiesSO.PlayerLayer);
        else
            _canWithDrawn = Physics2D.Raycast(new Vector2(_playerCheck.position.x, _playerCheck.position.y), Vector2.right, _withdrawnCheckDistance, _enemiesSO.PlayerLayer);

        return _canWithDrawn;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void SpawnBullet()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return;

        GameObject bullet = BulletPool.Instance.GetObjectInPool(GameConstants.TRUNK_BULLET);
        bullet.SetActive(true);
        bullet.transform.position = _shootPosition.position;

        BulletInfor info = new(GameConstants.TRUNK_BULLET, _isFacingRight);
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.BulletOnReceiveInfo, info);
        //Event của animation Attack
    }

    private void AllowUpdateWithDrawn()
    {
        _trunkRetreatState.AllowUpdate = true;
    }
}
