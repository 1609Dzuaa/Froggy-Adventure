using UnityEngine;

public class BaseState
{
    //Lớp cơ sở, mình có thể điều hướng các states ở các state con
    //Hoặc ở lớp Manager
    //Các GameObject khác muốn quản lý State thì kế thừa từ class này

    public virtual void EnterState(BaseStateManager stateManager) { }

    public virtual void ExitState(BaseStateManager stateManager) { }

    public virtual void UpdateState(BaseStateManager stateManager) { }

    public virtual void FixedUpdate(BaseStateManager stateManager) { }

}
