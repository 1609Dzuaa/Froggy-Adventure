using UnityEngine;

public class PigGotHitGreenState : MEnemiesGotHitState
{
    private PigManager _pigManager;
    private bool _allowUpdate;

    public bool AllowUpdate { set { _allowUpdate = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        _pigManager = (PigManager)charactersManager;
        _pigManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EPigState.gotHitGreen);
        _pigManager.GetRigidbody2D().velocity = Vector2.zero;
        //Debug.Log("GH Green");
    }

    public override void ExitState()
    {
        _allowUpdate = false;
    }

    public override void Update()
    {
        if (_allowUpdate)
            _pigManager.ChangeState(_pigManager.GetPigAttackRedState());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
