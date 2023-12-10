using UnityEngine;

public class BeeAttackState : MEnemiesAttackState
{
    private BeeManager _beeManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _beeManager = (BeeManager)charactersManager;
        _beeManager.GetRigidbody2D().velocity = Vector2.zero; //Cố định vị trí
        //Debug.Log("Bee Attack");
    }

    public override void ExitState()
    {
        _allowUpdate = false;
    }

    public override void Update()
    {
        if (_allowUpdate)
            if (CheckIfNeedChase())
                _beeManager.ChangeState(_beeManager.GetBeeChaseState());
    }

    private bool CheckIfNeedChase()
    {
        return _beeManager.MustAttack;
    }

    public override void FixedUpdate()
    {
        
    }
}
