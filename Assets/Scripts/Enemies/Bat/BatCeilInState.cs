using UnityEngine;

public class BatCeilInState : MEnemiesBaseState
{
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger("state", (int)GameEnums.EBatState.ceilIn);
        _batManager = (BatManager)charactersManager;
        //Cố định velo = 0 vì có thể còn thừa velo trước đó dẫn đến lúc ceil in vẫn move nhích từng tí
        _batManager.GetRigidbody2D().velocity = Vector2.zero;
        //Debug.Log("CI");
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
