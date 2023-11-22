using UnityEngine;

public class BatIdleState : BatBaseState
{
    private bool allowUpdate = false; //Cho nó chạy hết animation r mới Update
    private float distance; //Tính khcach giữa player và bat
    private float entryTime;

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.idle);
        entryTime = Time.time;
        Debug.Log("Idle");
    }

    public override void ExitState() 
    {
        allowUpdate = false;
    }

    public override void Update() 
    {
        if(allowUpdate)
        {
            //Coi Player và bat là 2 điểm => tính khoảng cách giữa 2 điểm trong khgian
            //Nếu ở trong tầm tấn công thì chuyển sang đuổi theo player
            //Nếu kh, check xem hết giờ ngủ ch
            distance = Vector2.Distance(_batStateManager.transform.position, _batStateManager.GetPlayer().position);
            if (_batStateManager.GetAttackRange() >= distance)
                _batStateManager.ChangState(_batStateManager.batChaseState);
            else if (Time.time - entryTime >= _batStateManager.GetRestTime() && !_batStateManager.batFlyState.GetHasFlyPatrol())
                _batStateManager.ChangState(_batStateManager.batFlyState); //Bay tuần tra
            else if (_batStateManager.batFlyState.GetHasFlyPatrol() && Time.time - entryTime >= _batStateManager.GetRestTime())
            {
                _batStateManager.ChangState(_batStateManager.batFlyState); //Bay về chỗ ngủ
            }
        }
    }

    public override void FixedUpdate() { }
}
