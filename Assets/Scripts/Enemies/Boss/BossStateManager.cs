using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameEnums;
using UnityEditor;

[System.Serializable]
public struct BossMinions
{
    public EBossMinions _minionName;
    public GameObject _minion;
}

/// <summary>
/// Boss sẽ có các skill:
/// Bật khiên lao về 1 phía, đụng tường thì tắt khiên vào state yếu
/// Bỏ chạy khỏi Player (state yếu)
/// Đứng yên, Dậm xuống triệu hồi quái sau đó đứng yên 1 chỗ trong vài giây
/// Đứng yên, Dậm xuống tạo Particle sau đó đứng yên 1 chỗ trong vài giây
/// </summary>

public class BossStateManager : MEnemiesManager
{
    [SerializeField] Vector2 _detectSize;
    [SerializeField] Vector2 _slamForceUp;
    [SerializeField] Vector2 _slamForceDown;
    [SerializeField] float _eachSlamTime;
    [SerializeField] GameObject _shield;

    [Header("Speed")]
    [SerializeField] float _retreatSpeed;
    [SerializeField] float _chargeSpeed;

    [Header("Minions")]
    [SerializeField] List<BossMinions> _listMinions = new();

    [Header("Spawn Position")]
    [SerializeField] Transform _spawnPos;

    [Header("Time")]
    [SerializeField] float _weakStateTime;
    [SerializeField] float _spawnDelay;

    BossNormalState _normalState = new();
    BossChargeState _chargeState = new();
    BossWallHitState _wallHitState = new();
    BossWeakState _weakState = new();
    BossShieldOnState _shieldOnState = new();
    BossSummonState _summonState = new();
    BossGotHitState _gotHitState = new();

    bool _enterBattle;
    bool _isVunerable;

    public BossNormalState NormalState { get => _normalState; set => _normalState = value; }

    public BossChargeState ChargeState { get => _chargeState; set => _chargeState = value; }

    public BossWallHitState WallHitState { get => _wallHitState; set => _wallHitState = value; }

    public BossWeakState WeakState { get => _weakState; set => _weakState = value; }

    public BossShieldOnState ShieldOnState { get => _shieldOnState; set => _shieldOnState = value; }

    public BossSummonState SummonState { get => _summonState; set => _summonState = value; }

    public BossGotHitState GotHitState { get => _gotHitState; set => _gotHitState = value; }

    public Transform PlayerRef { get => _playerCheck; }

    public float WeakStateTime { get => _weakStateTime; }

    public bool EnterBattle { get => _enterBattle; set => _enterBattle = value; }

    public bool HasGotHit { get => _hasGotHit; set => _hasGotHit = value; }

    public float RetreatSpeed { get => _retreatSpeed; }

    public float ChargeSpeed { get => _chargeSpeed; }

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
        _state = _normalState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DetectPlayer()
    {
        _hasDetectedPlayer = Physics2D.OverlapBox(transform.position, _detectSize, 0f, _enemiesSO.PlayerLayer);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(transform.position, _detectSize);
    }

    protected override void DrawRayDetectPlayer() { }

    protected override void DrawRayDetectGround() { }

    protected override void DrawRayDetectWall() { base.DrawRayDetectWall(); }

    protected override bool DetectWall() { return base.DetectWall(); }

    protected override void DetectGround() { }

    protected override void AllowAttackPlayer() { }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConstants.PLAYER_TAG))
            EventsManager.Instance.NotifyObservers(EEvents.PlayerOnTakeDamage, _isFacingRight);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isVunerable) return;

        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit)
        {
            _hasGotHit = true;
            ChangeState(_gotHitState);
            EventsManager.Instance.NotifyObservers(EEvents.PlayerOnJumpPassive, null);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public IEnumerator TurnOnShield()
    {
        Debug.Log("Called");
        yield return new WaitForSeconds(_weakStateTime);

        Debug.Log("Done Crt");
        ChangeState(_shieldOnState);
    }

    public IEnumerator Slam()
    {
        _rb.AddForce(Vector2.one * _slamForceUp);
        //_rb.AddForce(_isFacingRight ? Vector2.one * _slamForceUp : new Vector2(-1f, 1f) * _slamForceUp);

        yield return new WaitForSeconds(_eachSlamTime);

        _rb.AddForce(new Vector2(0f, -1f) * _slamForceDown);
        StartCoroutine(SpawnMinion());
    }

    public IEnumerator SpawnMinion()
    {
        int random = UnityEngine.Random.Range(0, Enum.GetValues(typeof(EBossMinions)).Length);
        //Debug.Log("Rd: " + random);
        SpawnSummonEffect();

        yield return new WaitForSeconds(_spawnDelay);

        switch (random)
        {
            case (int)EBossMinions.Plant:
                Instantiate(GetMinion(EBossMinions.Plant), _spawnPos.position, Quaternion.identity, null);
                Debug.Log("P");
                break;

            case (int)EBossMinions.Trunk:
                Instantiate(GetMinion(EBossMinions.Trunk), _spawnPos.position, Quaternion.identity, null);
                Debug.Log("T");
                break;

            case (int)EBossMinions.BigRock:
                Instantiate(GetMinion(EBossMinions.BigRock), _spawnPos.position, Quaternion.identity, null);
                Debug.Log("Br");
                break;

            case (int)EBossMinions.Chicken:
                Instantiate(GetMinion(EBossMinions.Chicken), _spawnPos.position, Quaternion.identity, null);
                Debug.Log("C");
                break;

            case (int)EBossMinions.Pig:
                Instantiate(GetMinion(EBossMinions.Pig), _spawnPos.position, Quaternion.identity, null);
                Debug.Log("PI");
                break;
        }
        EventsManager.Instance.NotifyObservers(EEvents.BossOnSummonMinion, _isFacingRight);
    }

    private GameObject GetMinion(EBossMinions minionName)
    {
        foreach (var minion in _listMinions)
            if (minion._minionName == minionName)
                return minion._minion;

        Debug.Log("No minion: " + minionName + " in Boss Pool");
        return null;
    }

    private void SpawnSummonEffect()
    {
        GameObject gObj = Pool.Instance.GetObjectInPool(EPoolable.BrownExplosion);
        gObj.SetActive(true);
        gObj.GetComponent<EffectController>().SetPosition(_spawnPos.position);
    }

    /// <summary>
    /// Các hàm dưới là event của các Animations
    /// </summary>

    private void ShieldOn()
    {
        _shield.SetActive(true);
    }

    private void ShieldOff()
    {
        _shield.SetActive(false);
    }

    private void BackToNormalState()
    {
        ChangeState(_normalState);
    }

    private void BackToWeakState()
    {
        ChangeState(_weakState);
    }

    private void CanBeVunerable()
    {
        _isVunerable = true;
    }

    private void CanNotBeVunerable()
    {
        _isVunerable = false;
    }
}
