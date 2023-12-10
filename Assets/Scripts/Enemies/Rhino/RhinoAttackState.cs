using UnityEngine;

public class RhinoAttackState : MEnemiesAttackState
{
    protected RhinoManager _rhinoManager;
    private bool _hasChangedState;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _rhinoManager = (RhinoManager)charactersManager;
    }

    public override void ExitState()
    {
        _hasChangedState = false;
    }

    public override void Update()
    {
        LogicUpdate();
    }

    private void LogicUpdate()
    {
        if (_rhinoManager.HasCollidedWall)
        {
            //Ưu tiên switch state WH hơn Idle khi đang Run
            _hasChangedState = true;
            //Xoá Invoke func vì có thể đã invoke Idle ở dưới nhưng lại đâm tường ở đây
            _rhinoManager.CancelInvoke();
            _rhinoManager.ChangeState(_rhinoManager.RhinoWallHitState);
        }
        else if (!_rhinoManager.HasDetectedPlayer && !_hasChangedState)
        {
            _hasChangedState = true;
            //Debug.Log("Here");
            _rhinoManager.Invoke("ChangeToIdle", _rhinoManager.RestDelay);
        }
    }

    public override void FixedUpdate()
    {
        Attack();
    }

    protected override void Attack()
    {
        if (_rhinoManager.GetIsFacingRight())
            _rhinoManager.GetRigidbody2D().velocity = new Vector2(_rhinoManager.GetChaseSpeed().x, _rhinoManager.GetRigidbody2D().velocity.y);
        else
            _rhinoManager.GetRigidbody2D().velocity = new Vector2(-_rhinoManager.GetChaseSpeed().x, _rhinoManager.GetRigidbody2D().velocity.y);
    }
}
