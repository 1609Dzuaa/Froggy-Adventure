using UnityEngine;

public class RhinoPatrolState : BaseState
{
    bool isFirstSawPlayer = true; //Lần đầu thấy Player thì đuổi theo luôn, 0 cần delay
    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.patrol);

            //Patrol animation is actually the same as Run animation but at a half frame-rate lower:)
            //Debug.Log("Patrol");
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
