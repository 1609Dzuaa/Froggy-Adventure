using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameEnums;
using UnityEngine.UI;

[System.Serializable]
public struct BossMinions
{
    public EBossMinions _minionName;
    public GameObject _minion;
}

/// <summary>
/// Boss sẽ HĐ như sau:
/// Player Enter Room thì bật state Charge lao thẳng vào Player
/// Nếu đâm tường thì về state weak, cho phép Player dmg nó
/// Ở state weak sẽ move chậm, chờ thgian từ coroutine để về state thường (Shield ON)
/// Ở state thường sẽ random 3 skills Charge, Summon Minions, Summon Particle, Summon Saws
/// Bố trí 2 Saws sao cho Saw phía trên dù Player có nhảy max vẫn sẽ dính để
/// Player phải học cách lui về 1 góc r đứng, nhảy xen kẽ
/// Muốn nó lực hơn nữa thì notify shake cam khi hành động (đâm tg, summon quái)
/// Cho boss lúc summon ra quái/particle thì cũng summon ra box support player (?)
/// </summary>

public class BossStateManager : MEnemiesManager
{
    [SerializeField] Vector2 _detectSize;
    [SerializeField] Vector2 _slamForceUp;
    [SerializeField] Vector2 _slamForceDown;
    [SerializeField] float _eachSlamTime;

    //[Header("HP")]
    [SerializeField] int _maxHP;

    [Header("Text Related")]
    [SerializeField] private Text _txtOverHead;
    [SerializeField] private Transform _txtPosition;
    [SerializeField] private float _delayTxtEnable;
    [SerializeField] private float _timeEnableTxt;
    [SerializeField] private float _timeEachIncrease;
    [SerializeField] private float _alphaEachIncrease;
    [SerializeField] private float _decreaseDelay;

    [Header("Weapon")]
    [SerializeField] GameObject _shield;
    [SerializeField] GameObject _particles;
    [SerializeField] GameObject _trap;

    [Header("Speed")]
    [SerializeField] float _retreatSpeed;
    [SerializeField] float _chargeSpeed;
    [SerializeField] float _particleOnSpeed;
    [SerializeField] float _trapSpeed;

    [Header("Minions")]
    [SerializeField] List<BossMinions> _listMinions = new();

    [Header("Buffs"), Tooltip("Spawn thêm buff support Player")]
    [SerializeField] GameObject _playerShield;
    [SerializeField] GameObject _playerAbsorb;

    [Header("Range")]
    [SerializeField] Transform _minRange;
    [SerializeField] Transform _maxRange;

    [Header("Position")]
    [SerializeField] Transform _spawnPos;
    [SerializeField] Transform _spawnTrapPos1; //2 Th này phải ref ở ngoài
    [SerializeField] Transform _spawnTrapPos2;
    [SerializeField] Transform _spawnTrapPos3;
    [SerializeField] Transform _spawnTrapPos4;
    [SerializeField] Transform _middleRoom;

    [Header("Time")]
    [SerializeField] float _weakStateTime;
    [SerializeField] float _particleStateTime;
    [SerializeField] float _delayBackToNormal;
    [SerializeField] float _spawnDelay; //delay để chạy hết spawn vfx
    [SerializeField] float _delaySpawnAppearVfx; //2 th này dùng khi Player đã hạ đc boss
    [SerializeField] float _delayBackToWeak;
    [SerializeField] float _deadDelay;
    [SerializeField] float _delaySpawnBuff;

    [Header("Dialog")]
    [SerializeField] Dialog _bossDialog;

    BossWaitState _waitState = new();
    BossNormalState _normalState = new();
    BossChargeState _chargeState = new();
    BossWallHitState _wallHitState = new();
    BossWeakState _weakState = new();
    BossShieldOnState _shieldOnState = new();
    BossSummonState _summonState = new();
    BossGotHitState _gotHitState = new();
    BossParticleState _particleState = new();
    BossTeleportState _teleportState = new();

    bool _enterBattle;
    bool _isVunerable;
    bool _isLastBreath;
    float _startTickSpawnBuff;

    #region Text Overhead Related
    bool _mustDecrease;
    float _entryTime;
    float _alpha;
    #endregion

    public BossChargeState ChargeState { get => _chargeState; set => _chargeState = value; }

    public BossWallHitState WallHitState { get => _wallHitState; set => _wallHitState = value; }

    public BossWeakState WeakState { get => _weakState; set => _weakState = value; }

    public BossSummonState SummonState { get => _summonState; set => _summonState = value; }

    public BossParticleState ParticleState { get=>_particleState; set => _particleState = value; }  

    public Transform PlayerRef { get => _playerCheck; }

    public Transform MiddleRoom { get => _middleRoom; }

    public Dialog BossDialog { get => _bossDialog; set => _bossDialog = value; }

    public bool EnterBattle { get => _enterBattle; set => _enterBattle = value; }

    public bool HasGotHit { get => _hasGotHit; set => _hasGotHit = value; }

    public bool IsLastBreath { get => _isLastBreath; set => _isLastBreath = value; }

    public float RetreatSpeed { get => _retreatSpeed; }

    public float ChargeSpeed { get => _chargeSpeed; }

    public float ParticleOnSpeed { get => _particleOnSpeed; }

    public int MaxHP { get => _maxHP; set => _maxHP = value; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnLevelCompleted, HandleWhenPlayerWin);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnLevelCompleted, HandleWhenPlayerWin);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _state = _waitState;
        _state.EnterState(this);
        _txtOverHead.enabled = false;
        Color textColor = _txtOverHead.color;
        textColor.a = 0f;
        _txtOverHead.color = textColor;
        _txtOverHead.transform.position = _txtPosition.position;
        _alpha = 0;
    }

    protected override void Update()
    {
        if (_txtOverHead.enabled && !_mustDecrease)
            IncreaseTextAlpha();

        if (_mustDecrease)
            DecreaseTextAlpha();

        //Debug.Log("Color: " + _txtOverHead.color.a);

        base.Update();
    }

    protected override void DetectPlayer()
    {
        _hasDetectedPlayer = Physics2D.OverlapBox(transform.position, _detectSize, 0f, _enemiesSO.PlayerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, _detectSize);
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

    public IEnumerator BackToNormal()
    {
        yield return new WaitForSeconds(_delayBackToNormal);

        ChangeState(_normalState);
        //Back về Normal State từ Summon || Particle State sau _delayBackToNormal (s)
    }

    public IEnumerator TurnOnShield()
    {
        yield return new WaitForSeconds(_weakStateTime);

        ChangeState(_shieldOnState);
        //Hết thgian ở weak state thì bật shield lên
    }

    public IEnumerator TurnOffParticle()
    {
        yield return new WaitForSeconds(_particleStateTime);

        _particles.SetActive(false);
        ChangeState(_normalState);
        _particleState.IsFirstEnterState = true;
        //Tắt particle khi hết thgian
    }

    public IEnumerator Slam(int state)
    {
        _rb.AddForce(Vector2.one * _slamForceUp);
        //_rb.AddForce(_isFacingRight ? Vector2.one * _slamForceUp : new Vector2(-1f, 1f) * _slamForceUp);

        yield return new WaitForSeconds(_eachSlamTime);

        _rb.AddForce(new Vector2(0f, -1f) * _slamForceDown);

        if (state == 0)
        {
            int minionOrTrap = UnityEngine.Random.Range(0, 2);

            StartCoroutine((minionOrTrap == 0 ? SpawnMinion() : SpawnTrap()));
        }
        else
            StartCoroutine(SpawnParticle());
    }

    private IEnumerator SpawnMinion()
    {
        int random = UnityEngine.Random.Range(0, Enum.GetValues(typeof(EBossMinions)).Length);
        //Debug.Log("Rd: " + random);
        SpawnSummonEffect(_spawnPos.position);

        yield return new WaitForSeconds(_spawnDelay);

        switch (random)
        {
            case (int)EBossMinions.Plant:
                Instantiate(GetMinion(EBossMinions.Plant), _spawnPos.position, Quaternion.identity, null);
                break;

            case (int)EBossMinions.Trunk:
                Instantiate(GetMinion(EBossMinions.Trunk), _spawnPos.position, Quaternion.identity, null);
                break;

            case (int)EBossMinions.Rhino:
                Instantiate(GetMinion(EBossMinions.Rhino), _spawnPos.position, Quaternion.identity, null);
                break;

            case (int)EBossMinions.Chicken:
                Instantiate(GetMinion(EBossMinions.Chicken), _spawnPos.position, Quaternion.identity, null);
                break;

            case (int)EBossMinions.Bunny:
                Instantiate(GetMinion(EBossMinions.Bunny), _spawnPos.position, Quaternion.identity, null);
                break;
        }
        EventsManager.Instance.NotifyObservers(EEvents.BossOnSummonMinion, _isFacingRight);
        SpawnBuff();
    }

    private IEnumerator SpawnTrap()
    {
        if (_playerCheck.position.x > _middleRoom.position.x)
        {
            SpawnSummonEffect(_spawnTrapPos1.position);
            SpawnSummonEffect(_spawnTrapPos2.position);
        }
        else
        {
            SpawnSummonEffect(_spawnTrapPos3.position);
            SpawnSummonEffect(_spawnTrapPos4.position);
        }

        yield return new WaitForSeconds(_spawnDelay);

        GameObject trap1 = Pool.Instance.GetObjectInPool(EPoolable.Saw);
        trap1.SetActive(true);
        trap1.GetComponentInChildren<SawController>().NeedMinMax = false;
        GameObject trap2 = Pool.Instance.GetObjectInPool(EPoolable.Saw);
        trap2.SetActive(true);
        trap2.GetComponentInChildren<SawController>().NeedMinMax = false;

        if (_playerCheck.position.x > _middleRoom.position.x)
        {
            trap1.GetComponentInChildren<SawController>().Speed = _trapSpeed;
            trap1.transform.position = _spawnTrapPos1.position;

            trap2.GetComponentInChildren<SawController>().Speed = -_trapSpeed;
            trap2.transform.position = _spawnTrapPos2.position;
        }
        else
        {
            trap1.GetComponentInChildren<SawController>().Speed = -_trapSpeed;
            trap1.transform.position = _spawnTrapPos4.position;

            trap2.GetComponentInChildren<SawController>().Speed = _trapSpeed;
            trap2.transform.position = _spawnTrapPos3.position;
        }

        SpawnBuff();
        //Dựa vào vị trí Player mà spawn Trap move ngc hướng nhau từ 2 vị trí trong Room
    }

    private GameObject GetMinion(EBossMinions minionName)
    {
        foreach (var minion in _listMinions)
            if (minion._minionName == minionName)
                return minion._minion;

        Debug.Log("No minion: " + minionName + " in Boss Pool");
        return null;
    }

    private void SpawnBuff()
    {
        if (Time.time - _startTickSpawnBuff >= _delaySpawnBuff)
        {
            int random = UnityEngine.Random.Range(0, 2);
            float xPos = UnityEngine.Random.Range(_minRange.position.x, _maxRange.position.x);
            float yPos = UnityEngine.Random.Range(_minRange.position.y, _maxRange.position.y);
            Vector3 spawnPos = new(xPos, yPos, 0f);

            Instantiate((random > 0) ? _playerShield : _playerAbsorb, spawnPos, Quaternion.identity);
            _startTickSpawnBuff = Time.time;
        }
    }

    private void SpawnSummonEffect(Vector3 pos)
    {
        GameObject gObj = Pool.Instance.GetObjectInPool(EPoolable.RedExplode);
        gObj.SetActive(true);
        gObj.transform.position = pos;
    }

    private IEnumerator SpawnParticle()
    {
        yield return new WaitForSeconds(_spawnDelay);

        _particles.SetActive(true);
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

    #region Introduce Boss Related

    public void StartIntroduceText()
    {
        StartCoroutine(Enable());
    }

    private IEnumerator Enable()
    {
        yield return new WaitForSeconds(_delayTxtEnable);

        _txtOverHead.enabled = true;
        _entryTime = Time.time; //Bấm giờ để tăng độ Alpha theo thgian cho Text

        StartCoroutine(DisableText());
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(_timeEnableTxt);

        _mustDecrease = true;
    }

    private void IncreaseTextAlpha()
    {
        if (_alpha >= 1)
            return;
        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _alpha += _alphaEachIncrease;
            Color textColor = _txtOverHead.color;
            textColor.a = _alpha;
            _txtOverHead.color = textColor;
            //Debug.Log("Color: " + _txtOverHead.color.a);
            _entryTime = Time.time;
        }
    }

    private void DecreaseTextAlpha()
    {
        if (_alpha <= 0)
            return;
        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _alpha -= _alphaEachIncrease;
            Color textColor = _txtOverHead.color;
            textColor.a = _alpha;
            _txtOverHead.color = textColor;
            //Debug.Log("Color: " + _txtOverHead.color.a);
            _entryTime = Time.time;
        }
    }

    private void HandleWhenPlayerWin(object obj)
    {
        ChangeState(_teleportState);
    }

    public IEnumerator BackToWeak()
    {
        yield return new WaitForSeconds(_delaySpawnAppearVfx);

        SpawnAppearVfx();
        SoundsManager.Instance.PlaySfx(ESoundName.PlayerDashSfx, 0.5f);
        yield return new WaitForSeconds(_delayBackToWeak);

        ChangeState(_weakState);
    }

    private void SpawnAppearVfx()
    {
        GameObject gObj = Pool.Instance.GetObjectInPool(EPoolable.BossAppearVfx);
        gObj.SetActive(true);
        gObj.transform.position = transform.position;
    }

    public IEnumerator Dead()
    {
        yield return new WaitForSeconds(_deadDelay);

        SpawnDeadVfx();
        SoundsManager.Instance.PlaySfx(ESoundName.BossDeadSfx, 1.0f);
        Destroy(gameObject);
    }

    private void SpawnDeadVfx()
    {
        GameObject gObj = Pool.Instance.GetObjectInPool(EPoolable.BossDeadVfx);
        gObj.SetActive(true);
        gObj.transform.position = transform.position;
    }

    protected override void HandleIfBossDie(object obj) { }

    #endregion

    /// <summary>
    /// Các hàm dưới là event của các Animations
    /// và Signal Emitter của Timeline
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

    public void AllowEnterBattle()
    {
        _enterBattle = true;
    }
}
