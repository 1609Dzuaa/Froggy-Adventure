using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogManager : NMEnemiesManager
{
    //Điều chỉnh collider khi Defend    
    [Header("Time")]
    [SerializeField] private float _spikeInDelay;

    private HedgehogIdleState _hedgehogIdleState = new(); //thừa, sửa sau
    private HedgehogSpikeIdleState _hedgehogSpikeIdle = new();

    public float SpikeInDelay { get { return _spikeInDelay; } }

    protected override void Start()
    {
        NMEnemiesIdleState = _hedgehogIdleState;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void ChangeToSpikeIdle()
    {
        ChangeState(_hedgehogSpikeIdle);
        //Event func của animation SpikeOut
    }

    private void ChangeToSpikeIn()
    {
        _anim.SetInteger("state", (int)EnumState.EHedgehogState.spikeIn);
        //Dùng Invoke khi 0 detect Player trong _spikeInDelay (s)
    }

    private void ChangeToIdle()
    {
        ChangeState(NMEnemiesIdleState);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Chỉ giết đc <=> 0 mọc gai
        if (_state is HedgehogIdleState)
            base.OnTriggerEnter2D(collision);
    }
}
