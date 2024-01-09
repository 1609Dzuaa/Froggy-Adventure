using UnityEngine;

public class TrunkWithdrawnState : MEnemiesBaseState
{
    TrunkManager _trunkManager;
    bool _allowUpdate;

    public bool AllowUpdate { set {  _allowUpdate = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ETrunkState.withdrawn);
        HandleWithdrawn();
        //Debug.Log("Rt");
    }

    public override void ExitState()
    {
        base.ExitState();
        _allowUpdate = false;
    }

    public override void Update()
    {
        if (_allowUpdate)
            if (CheckIfCanAttack())
                _trunkManager.ChangeState(_trunkManager.GetTrunkAttackState());
            else if (CheckIfCanIdle())
                _trunkManager.ChangeState(_trunkManager.GetTrunkIdleState());
    }

    private bool CheckIfCanAttack()
    {
        return _trunkManager.HasDetectedPlayer;
    }

    private bool CheckIfCanIdle()
    {
        return !_trunkManager.HasDetectedPlayer;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void HandleWithdrawn()
    {
        if (_trunkManager.GetIsFacingRight())
            _trunkManager.GetRigidbody2D().velocity = _trunkManager.WithdrawnForce * new Vector2(-1f, 1f);
        else
            _trunkManager.GetRigidbody2D().velocity = _trunkManager.WithdrawnForce;
    }
}
