using UnityEngine;

public abstract class MrBaseState
{
    protected MrStateManager _mrStateManager;

    //Mỗi lần EnterState ở class con chỉ cần gọi base.EnterState
    public virtual void EnterState(MrStateManager mrStateManager) { _mrStateManager = mrStateManager; }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdate() { }
}
