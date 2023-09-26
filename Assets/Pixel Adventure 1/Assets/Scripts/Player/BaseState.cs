using UnityEngine;

public abstract class BaseState
{
    //Lớp cơ sở, mình có thể điều hướng các states ở các state con
    //Hoặc ở lớp Manager
    public abstract void EnterState(StateManager stateManager);

    public abstract void ExitState(StateManager stateManager);

    public abstract void UpdateState(StateManager stateManager);

    public abstract void FixedUpdate(StateManager stateManager);

}
