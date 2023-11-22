using UnityEngine;

public abstract class BatBaseState 
{
    protected BatStateManager _batStateManager;

    public virtual void EnterState(BatStateManager batStateManager) { _batStateManager = batStateManager; }

    public virtual void ExitState() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
