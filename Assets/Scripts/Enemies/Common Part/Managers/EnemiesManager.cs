﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;
using static GameEnums;

public class EnemiesManager : CharactersManager
{
    [Header("Player Check")]
    [SerializeField] protected Transform _playerCheck;

    [Header("SO")]
    [SerializeField] protected EnemiesStats _enemiesSO;

    protected bool _hasDetectedPlayer;
    protected bool _hasGotHit; //Đánh dấu bị Hit, tránh Trigger nhiều lần
    protected Collider2D _collider2D;
    protected SpriteRenderer _spriteRenderer;
    protected bool _hasNotified; //Chỉ dành cho các Enemy cần Tutor
    protected bool _notPlayDeadSfx; //Boss chet thi 0 play Sfx
    protected RaycastHit2D _hit2D;
    protected bool _bountyMarked;

    #region GETTER

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    public Collider2D GetCollider2D { get { return _collider2D; } set { _collider2D = value; } }

    public SpriteRenderer GetSpriteRenderer { get => _spriteRenderer; }

    public EnemiesStats EnemiesSO { get => _enemiesSO; }

    public bool NotPlayDeadSfx { get => _notPlayDeadSfx; }

    #endregion

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

    protected virtual void OnEnable()
    {
        EventsManager.SubcribeToAnEvent(EEvents.ObjectOnRestart, OnRestartID);
        EventsManager.SubcribeToAnEvent(EEvents.BossOnSummonMinion, ReceiveBossCommand);
        EventsManager.SubcribeToAnEvent(EEvents.BossOnDie, HandleIfBossDie);
        EventsManager.SubcribeToAnEvent(EEvents.OnBountyMarked, BountyMarked);
        //Debug.Log("Subbed");
    }

    protected virtual void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.ObjectOnRestart, OnRestartID);
        EventsManager.UnsubscribeToAnEvent(EEvents.BossOnSummonMinion, ReceiveBossCommand);
        EventsManager.UnsubscribeToAnEvent(EEvents.BossOnDie, HandleIfBossDie);
        EventsManager.UnsubscribeToAnEvent(EEvents.OnBountyMarked, BountyMarked);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        if (Math.Abs(transform.rotation.eulerAngles.y) >= 180f)
            _isFacingRight = true;
        //Debug.Log("IfR, yAngles: " + _isFacingRight + ", " + transform.rotation.eulerAngles.y);
    }

    protected override void Update()
    {
        base.Update();
        DrawRayDetectPlayer();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        DetectPlayer();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYER_TAG))
            EventsManager.NotifyObservers(EEvents.PlayerOnTakeDamage, _isFacingRight);
    }

    protected virtual void DetectPlayer()
    {
        if (BuffsManager.Instance.GetBuff(EBuffs.Invisible).IsActivating)
        {
            _hasDetectedPlayer = false;
            return;
        }

        _hit2D = Physics2D.Raycast(_playerCheck.position, (!_isFacingRight) ? Vector2.left : Vector2.right, _enemiesSO.PlayerCheckDistance, _enemiesSO.PlayerLayer);

        if (_hit2D)
            _hasDetectedPlayer = _hit2D.collider.CompareTag(PLAYER_TAG);
        else
            _hasDetectedPlayer = false;
    }

    protected virtual void DrawRayDetectPlayer()
    {
        if (_hasDetectedPlayer)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.left * _enemiesSO.PlayerCheckDistance, Color.red);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.right * _enemiesSO.PlayerCheckDistance, Color.red);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_playerCheck.position, Vector2.left * _enemiesSO.PlayerCheckDistance, Color.green);
            else
                Debug.DrawRay(_playerCheck.position, Vector2.right * _enemiesSO.PlayerCheckDistance, Color.green);
        }
    }

    protected virtual void SelfDestroy()
    {
        Destroy(gameObject);
    }

    protected void OnRestartID(object obj)
    {
        _ID = null;
    }

    protected virtual void HandleIfBossDie(object obj) { }

    protected void ReceiveBossCommand(object obj)
    {
        if ((bool)obj != _isFacingRight)
            FlippingSprite();
        _isFacingRight = (bool)obj;
        //Debug.Log("Received: " + _isFacingRight);
    }

    protected void BountyMarked(object obj) => _bountyMarked = true;

    protected void SpawnBountyIfMarked()
    {
        if (_bountyMarked)
        {
            int start = (int)EPoolable.Apple;
            int end = (int)EPoolable.Strawberry;
            int random = UnityEngine.Random.Range(start, end + 1);

            //vfx
            GameObject vfx = Pool.Instance.GetObjectInPool(EPoolable.BountyAppearVfx);
            vfx.SetActive(true);
            vfx.transform.position = transform.position;

            //bounty
            //nên giới hạn chỉ random 3 loại có trong map đó 
            GameObject go = Pool.Instance.GetObjectInPool((EPoolable)random);
            go.SetActive(true);
            go.transform.position = transform.position;
        }
        //Debug.Log("marked: " + _bountyMarked);
    }

    protected void SpawnRewardForPlayer()
    {
        int sCoinGiven = UnityEngine.Random.Range(_enemiesSO.MinSilverGiven, _enemiesSO.MaxSilverGiven + 1);
        int gCoinGiven = UnityEngine.Random.Range(_enemiesSO.MinGoldGiven, _enemiesSO.MaxGoldGiven + 1);
        int SilverOrCoin = UnityEngine.Random.Range(0, 2);

        CoinInfo info = new CoinInfo((SilverOrCoin == 0) ? ECurrency.Silver : ECurrency.Gold, (SilverOrCoin == 0) ? sCoinGiven : gCoinGiven);
        EventsManager.NotifyObservers(EEvents.OnRewardCoin, info);
    }

    protected void SpawnDeathFX(EPoolable fxName)
    {
        GameObject deathFx = Pool.Instance.GetObjectInPool(fxName);
        deathFx.transform.position = transform.position;
        deathFx.SetActive(true);
    }
}
