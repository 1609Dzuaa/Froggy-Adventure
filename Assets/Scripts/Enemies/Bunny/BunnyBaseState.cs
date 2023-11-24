using UnityEngine;

public class BunnyBaseState
{
    protected BunnyStateManager _bunnyStateManager;

    public virtual void EnterState(BunnyStateManager bunnyStateManager) { _bunnyStateManager = bunnyStateManager; }

    public virtual void ExitState() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
