using UnityEngine;

public class BatChaseState : BatBaseState
{
    private float distance;

    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.chase);
        Debug.Log("Chase");
    }

    public override void ExitState() { }

    public override void UpdateState() 
    {
        //Debug.Log("Here");
        HandleChasingPlayer();

        BoundariesCheck();
    }

    public override void FixedUpdate() { }


    private void HandleChasingPlayer()
    {
        distance = _batStateManager.transform.position.x - _batStateManager.GetPlayer().position.x;
        if (distance < 0 && !_batStateManager.GetIsFacingRight())
            _batStateManager.FlipRight();
        else if (distance > 0 && _batStateManager.GetIsFacingRight())
            _batStateManager.FlipLeft();

        //Move vật thể theo target
        _batStateManager.transform.position = Vector2.MoveTowards(_batStateManager.transform.position, _batStateManager.GetPlayer().position, _batStateManager.GetChaseSpeed() * Time.deltaTime);
    }

    private void BoundariesCheck()
   {
       if (_batStateManager.transform.position.x >= _batStateManager.GetMaxPointRight().position.x)
       {
            _batStateManager.FlipLeft();
            _batStateManager.ChangState(_batStateManager.batRetreatState);
       }
       else if(_batStateManager.transform.position.x <= _batStateManager.GetMaxPointLeft().position.x)
       {
            _batStateManager.FlipRight();
            _batStateManager.ChangState(_batStateManager.batRetreatState);
       }
   }
}
