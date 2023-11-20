using UnityEngine;

public class BatBaseState 
{
    protected BatStateManager _batStateManager;

    public virtual void EnterState(BatStateManager batStateManager) { _batStateManager = batStateManager; }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdate() { }
}
