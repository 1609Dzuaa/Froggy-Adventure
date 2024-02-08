using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : CharactersManager
{
    private BossNormalState _normalState = new();

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
}
