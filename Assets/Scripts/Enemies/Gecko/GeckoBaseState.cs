using UnityEngine;

public abstract class GeckoBaseState
{
    protected GeckoStateManager _geckoStateManager;

    public virtual void EnterState(GeckoStateManager geckoStateManager) { _geckoStateManager = geckoStateManager; }

    public virtual void ExitState() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
