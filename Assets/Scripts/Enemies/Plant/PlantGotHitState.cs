using UnityEngine;

public class PlantGotHitState : PlantBaseState
{
    private float Zdegree = 0f;
    private float lastRotateTime;

    public override void EnterState(PlantStateManager plantStateManager)
    {
        base.EnterState(plantStateManager);
        _plantStateManager.GetAnimator().SetInteger("state", (int)EnumState.EPlantState.gotHit);
        lastRotateTime = Time.time;
        //Debug.Log("GH");
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        if(Time.time - lastRotateTime >= _plantStateManager.GetTimeEachRotate())
        {
            Zdegree -= _plantStateManager.GetDegreeEachRotation();
            _plantStateManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }
}
