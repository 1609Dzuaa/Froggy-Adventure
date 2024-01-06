using UnityEngine;

public class TrunkAttackState : MEnemiesAttackState
{
    private TrunkManager _trunkManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //base.EnterState(charactersManager);
        _charactersManager = charactersManager;
        _trunkManager = (TrunkManager)charactersManager;
        _trunkManager.Animator.SetInteger("state", (int)GameEnums.ETrunkState.attack);
        _trunkManager.GetRigidbody2D().velocity = Vector2.zero;
        //Debug.Log("Attack");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (!_trunkManager.HasDetectedPlayer)
        {
            //Debug.Log("Here");
            _trunkManager.ChangeState(_trunkManager.GetTrunkIdleState());
        }
        //base.Update();
    }

    public override void FixedUpdate()
    {
        
    }

    protected override void Attack()
    {
        
    }
}
