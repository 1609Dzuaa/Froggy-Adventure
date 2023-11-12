using UnityEngine;

public class RhinoIdleState : BaseState
{
    bool isFirstSawPlayer = true; //Lần đầu thấy Player thì đuổi theo luôn, 0 cần delay
    bool hasChangeState = false; //Đảm bảo chỉ change state 1 lần duy nhất tránh việc
    //frame trước đã change sang run nhưng frame lại lại change sang patrol
    bool canRdDirection = false; //Check nếu Hitwall khi patrol thì 0 cho random hướng mà phải patrol hướng ngược lại

    public void SetCanRdDirection(bool para) { this.canRdDirection = para; }

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager = (RhinoStateManager)_baseStateManager;
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.idle);
            hasChangeState = false;
            rhinoStateManager.SetChangeRightDirection(Random.Range(0, 2));
            //Debug.Log("Idle"); //Keep this, use for debugging change state
        }
    }

    public override void ExitState()
    {
        canRdDirection = false;
    }

    public override void UpdateState()
    {
        if (rhinoStateManager.GetHasDetectedPlayer() && isFirstSawPlayer &&!hasChangeState)
        {
            isFirstSawPlayer = false;
            hasChangeState = true;
            //rhinoStateManager.SpawnWarning();
            rhinoStateManager.ChangeState(rhinoStateManager.rhinoRunState);
        }
        else if (rhinoStateManager.GetHasDetectedPlayer() && !hasChangeState)
        {
            hasChangeState = true;
            //rhinoStateManager.SpawnWarning();
            rhinoStateManager.Invoke("AllowChasingPlayer", rhinoStateManager.GetChasingDelay()); //Delay 0.3s
        }
        else if(!hasChangeState && canRdDirection)
        {
            //Nếu 0 detect ra player và 0 đụng tường thì patrol sau restDur (s)
            //và random true false để đổi hướng patrol tiếp
            hasChangeState = true;
            rhinoStateManager.Invoke("AllowPatrol1", rhinoStateManager.GetRestDuration());
        }
        else if(!hasChangeState) //Can't Random Direction to patrol
        {
            //Patrol theo hướng mặt (0 random)
            hasChangeState = true;
            rhinoStateManager.Invoke("AllowPatrol2", rhinoStateManager.GetRestDuration());
        }
    }

    public override void FixedUpdate()
    {

    }
}
