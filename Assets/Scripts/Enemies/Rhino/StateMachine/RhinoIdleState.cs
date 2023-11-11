using UnityEngine;

public class RhinoIdleState : BaseState
{
    bool isFirstSawPlayer = true; //Lần đầu thấy Player thì đuổi theo luôn, 0 cần delay
    bool hasChangeState = false; //Đảm bảo chỉ change state run 1 lần duy nhất

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager = (RhinoStateManager)_baseStateManager;
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.idle);
            hasChangeState = false;
            rhinoStateManager.SetChangeRightDirection(Random.Range(0, 2));
            Debug.Log("Idle"); //Keep this, use for debugging change state
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (rhinoStateManager.GetHasDetectedPlayer() && isFirstSawPlayer)
        {
            isFirstSawPlayer = false;
            //rhinoStateManager.SpawnWarning();
            rhinoStateManager.ChangeState(rhinoStateManager.rhinoRunState);
        }
        else if (rhinoStateManager.GetHasDetectedPlayer() && !hasChangeState)
        {
            hasChangeState = true;
            //rhinoStateManager.SpawnWarning();
            rhinoStateManager.Invoke("AllowChasingPlayer", 0.5f); //Delay 0.5s
        }
        else if(!hasChangeState)
        {
            //Nếu 0 detect ra player thì patrol sau restDur (s)
            //và random true false để đổi hướng patrol tiếp
            hasChangeState = true;
            //Debug.Log("Right?: " + changeRightDirection);
            rhinoStateManager.Invoke("AllowPatrol", rhinoStateManager.GetRestDuration());
        }
    }

    public override void FixedUpdate()
    {

    }
}
