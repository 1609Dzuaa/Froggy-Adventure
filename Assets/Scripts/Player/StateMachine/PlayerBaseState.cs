using UnityEngine;

public abstract class PlayerBaseState
{
    //Lớp cơ sở, mình có thể điều hướng các states ở các state con
    //Hoặc ở lớp Manager
    //Các GameObject khác muốn quản lý State thì kế thừa từ class này

    //EnterState nhận tham số đầu vào là kiểu StateManager của object
    //=>Để lấy được đầy đủ thông tin của object đó

    //Chứa kiểu StateManager của object đó và có thể truy cập ở các state kế thừa
    protected PlayerStateManager _playerStateManager;

    public virtual void EnterState(PlayerStateManager playerStateManager) { _playerStateManager = playerStateManager; }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdate() { }

}
