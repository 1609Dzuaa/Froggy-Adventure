using UnityEngine;

public class BatIdleState : MEnemiesIdleState
{
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        //Debug.Log("Idle " + _allowAttack);
    }

    public override void ExitState() 
    {
        base.ExitState();
    }

    public override void Update()
    {
        //Coi Player và Bat là 2 điểm => tính khoảng cách giữa 2 điểm trong khgian
        //Nếu ở trong tầm tấn công thì chuyển sang đuổi theo player
        //Nếu kh, check xem hết giờ ngủ ch
        if (CheckIfCanAttack())
            _batManager.ChangeState(_batManager.BatAttackState);
        else if (Time.time - _entryTime >= _batManager.GetRestTime())
            _batManager.ChangeState(_batManager.BatPatrolState);
    }

    public override void FixedUpdate() { }
}
