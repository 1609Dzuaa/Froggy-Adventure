using UnityEngine;

public class BaseState
{
    //Lớp cơ sở, mình có thể điều hướng các states ở các state con
    //Hoặc ở lớp Manager
    //Các GameObject khác muốn quản lý State thì kế thừa từ class này

    //Tham số là kiểu MonoBehaviour để lấy Context 
    //=>Đại khái là lấy được đầy đủ thông tin của vật thể

    //Tạo sẵn những biến tĩnh kiểu là kiểu ...StateManager ?
    //Mục đích:
    //Nhằm cho các state biến dùng chung để reference context class
    static protected PlayerStateManager playerStateManager;
    protected BaseStateManager baseStateManager;

    public virtual void EnterState(BaseStateManager _baseStateManager) { }//playerStateManager = (PlayerStateManager)_baseStateManager; }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }

    public virtual void FixedUpdate() { }

}
