using UnityEngine;

public class RhinoIdleState : BaseState
{
    bool isFirstSawPlayer = true; //Lần đầu thấy Player thì đuổi theo luôn, 0 cần delay
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager = (RhinoStateManager)_baseStateManager;
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.idle);
            
            //Debug.Log("Idle"); //Keep this, use for debugging change state
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
            rhinoStateManager.ChangeState(rhinoStateManager.rhinoRunState);
        }
        else if (rhinoStateManager.GetHasDetectedPlayer())
            rhinoStateManager.Invoke("AllowChasingPlayer", 0.5f); //Delay 0.5s
    }

    public override void FixedUpdate()
    {

    }
}
