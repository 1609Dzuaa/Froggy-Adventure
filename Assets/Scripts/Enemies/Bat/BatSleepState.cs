using UnityEngine;

public class BatSleepState : MEnemiesBaseState
{
    private float _entryTime;
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _batManager.Animator.SetInteger("state", (int)EnumState.EBatState.sleep);
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
        if (Time.time - _entryTime >= _batManager.SleepTime)
            return true;
        return false;
    }

    public override void FixedUpdate() { }
}
