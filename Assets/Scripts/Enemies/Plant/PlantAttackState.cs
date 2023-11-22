using UnityEngine;

public class PlantAttackState : PlantBaseState
{
    public override void EnterState(PlantStateManager plantStateManager)
    {
        base.EnterState(plantStateManager);
        _plantStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlantState.attack);
        //Debug.Log("Attack");
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        if (!_plantStateManager.GetHasDetectedPlayer())
            _plantStateManager.ChangeState(_plantStateManager.plantIdleState);
    }
}
