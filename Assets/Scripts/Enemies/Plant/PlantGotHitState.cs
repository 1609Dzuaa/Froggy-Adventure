using UnityEngine;

public class PlantGotHitState : PlantBaseState
{
    public override void EnterState(PlantStateManager plantStateManager)
    {
        base.EnterState(plantStateManager);
        _plantStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlantState.gotHit);
        Debug.Log("GH");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }
}
