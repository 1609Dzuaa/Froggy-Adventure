using UnityEngine;

public class PlantIdleState : PlantBaseState
{
    public override void EnterState(PlantStateManager plantStateManager)
    {
        base.EnterState(plantStateManager);
        _plantStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlantState.idle);
        //Debug.Log("Idle");
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
    {
        if(_plantStateManager.GetHasDetectedPlayer())
            _plantStateManager.ChangeState(_plantStateManager.plantAttackState);
    }

}
