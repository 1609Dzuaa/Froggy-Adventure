using UnityEngine;

public class BatSleepState : MEnemiesBaseState
{
    private float _entryTime;
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _batManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBatState.sleep);
        _entryTime = Time.time;
        //Debug.Log("Sleep");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (CheckIfOverSleepTime())
            _batManager.ChangeState(_batManager.BatCeilOutState);
    }

    private bool CheckIfOverSleepTime()
    {
        return Time.time - _entryTime >= _batManager.SleepTime;
    }

    public override void FixedUpdate() { }
}
