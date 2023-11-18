using UnityEngine;

public abstract class PlantBaseState
{
    protected PlantStateManager _plantStateManager;

    public virtual void EnterState(PlantStateManager plantStateManager) { _plantStateManager = plantStateManager; }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdate() { }
}
