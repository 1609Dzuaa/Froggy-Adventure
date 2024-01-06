using UnityEngine;

public class BatCeilOutState : MEnemiesBaseState
{
    private bool _allowIdle;
    private BatManager _batManager;

    public bool AllowIdle { set { _allowIdle = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _mEnemiesManager.Animator.SetInteger("state", (int)GameEnums.EBatState.ceilOut);
        //Debug.Log("CO");
    }

    public override void ExitState() { _allowIdle = false; }

    public override void Update() 
    {
        if (_allowIdle)
            _batManager.ChangeState(_batManager.BatIdleState);
    }

    public override void FixedUpdate() { }
}
