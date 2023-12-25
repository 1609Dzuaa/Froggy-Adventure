using UnityEngine;

public class SnailAttackState : MEnemiesAttackState
{
    private SnailManager _snailManager;
    private bool _hasChangedState;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        //Chỉnh lại Box Trigger khi Defend
    }

    public override void ExitState()
    {
        base.ExitState();
        _hasChangedState = false;
    }

    public override void Update()
    {
        //Sometimes, prob here:D
        if (!_snailManager.HasDetectedPlayer && !_hasChangedState)
        {
            _hasChangedState = true;
            _snailManager.Invoke("ChangeToIdle", _snailManager.DelayIdleTime);
        }
        else
        {
            _hasChangedState = false;
            _snailManager.CancelInvoke();
        }
        //base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
