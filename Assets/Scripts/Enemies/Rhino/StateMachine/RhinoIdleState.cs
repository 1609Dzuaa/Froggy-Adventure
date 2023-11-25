using UnityEngine;

public class RhinoIdleState : RhinoBaseState
{
    private bool hasChangedState = false; //Đảm bảo chỉ change state 1 lần duy nhất tránh việc
    //frame trước đã change sang run nhưng frame lại lại change sang patrol
    //bool canRdDirection = false; //Check nếu Hitwall khi patrol thì 0 cho random hướng mà phải patrol hướng ngược lại

    private float entryTime;

    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.idle);
        hasChangedState = false;
        //Set như này để tránh ma sát mà nó 0 dừng lại hẳn
        _rhinoStateManager.GetRigidBody2D().velocity = Vector2.zero;
        entryTime = Time.time;
        //Debug.Log("Idle"); //Keep this, use for debugging change state
    }

    public override void ExitState()
    {
        hasChangedState = false;
    }

    public override void Update()
    {
        if (Time.time - entryTime >= _rhinoStateManager.GetRestTime())
        {
            //Set co the random kh ?
            _rhinoStateManager.ChangeState(_rhinoStateManager.rhinoPatrolState);
        }
        else if (_rhinoStateManager.GetHasDetectedPlayer() && !hasChangedState)
        {
            hasChangedState = true;
            _rhinoStateManager.Invoke("AllowChasingPlayer", _rhinoStateManager.GetChasingDelay()); //Delay 1 xíu
        }
        else
        {

        }
    }

    public override void FixedUpdate()
    {

    }
}
