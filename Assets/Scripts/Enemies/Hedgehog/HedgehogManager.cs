using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogManager : NMEnemiesManager
{
    //Điều chỉnh collider khi Defend    
    [Header("Time")]
    [SerializeField] private float _spikeInDelay;

    [Header("Collider2D")]
    [SerializeField] private Vector2 _sizeIncrease;

    private HedgehogSpikeIdleState _hedgehogSpikeIdle = new();
    private BoxCollider2D _boxCollider2D;
    private Vector2 _prevCollider2DSize;
    static string _ID;

    public float SpikeInDelay { get { return _spikeInDelay; } }

    public Vector2 getSizeIncrease { get { return _sizeIncrease; } }

    public BoxCollider2D getBoxCollider2D { get { return _boxCollider2D; } }

    protected override void Awake()
    {
        base.Awake();
        if (_ID == null)
            _ID = Guid.NewGuid().ToString(); //Đảm bảo mỗi bullet sẽ có ID riêng
        if (PlayerPrefs.HasKey("Deleted" + _ID))
            gameObject.SetActive(false);
        Debug.Log("ID: " + _ID);
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        //Muốn chỉnh collider trục y thì cần chỉnh thêm offset
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void ChangeToSpikeIdle()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
        {
            ChangeState(_nmEnemiesIdleState);
            return;
        }

        _prevCollider2DSize = _boxCollider2D.size;
        ChangeState(_hedgehogSpikeIdle);
        //Event func của animation SpikeOut
    }

    private void ChangeToSpikeIn()
    {
        _anim.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EHedgehogState.spikeIn);
        //Dùng Invoke khi 0 detect Player trong _spikeInDelay (s)
    }

    private void ChangeToIdle()
    {
        ChangeState(_nmEnemiesIdleState);
        _boxCollider2D.size = _prevCollider2DSize;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Chỉ giết đc <=> 0 mọc gai
        if (_state is NMEnemiesIdleState)
        {
            if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasGotHit && _state is not NMEnemiesGotHitState)
            {
                //Thêm ĐK: _state is not NMEnemiesGotHitState vì:
                //có thể enemy này dính đòn từ bullet => rotate z khi GotHit
                //và Trigger collider của Player dẫn đến JumpPassive
                _hasGotHit = true;
                EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnJumpPassive, null);
                ChangeState(_nmEnemiesGotHitState);
                PlayerPrefs.SetString("Deleted" + _ID, "deleted");
            }
            else if (collision.CompareTag(GameConstants.DEAD_ZONE_TAG) && !_hasGotHit && _state is not NMEnemiesGotHitState)
            {
                _hasGotHit = true;
                ChangeState(_nmEnemiesGotHitState);
            }
            //base.OnTriggerEnter2D(collision);
        }
    }
}
