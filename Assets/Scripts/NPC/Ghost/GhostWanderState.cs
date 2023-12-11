using UnityEngine;

public class GhostWanderState : CharacterBaseState
{
    private GhostManager _ghostManager;
    private float _entryTime;
    private bool _hasChangeDirection;
    private bool _canDisappear; //Nếu đã switch sang Idle từ state này r thì có thể disappear ở lần tới
    private bool _canRd = true;
    private int _rdLeftRightDir;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger("state", (int)EnumState.EGhostState.wander);
        Debug.Log("Can Rd: " + _canRd);
        if (_canRd)
            HandleRandomLeftRightDirection();
        _entryTime = Time.time;
        //Debug.Log("Wander");
    }

    public override void ExitState()
    {
        //Trước khi rời state, check nếu va min/max thì lần enter state wander kế tiếp
        //sẽ 0 đc rd mà phải đi hướng isFacingRight hiện tại
        if (_hasChangeDirection)
            _canRd = false;
        else 
            _canRd = true;
        _hasChangeDirection = false;
    }

    public override void Update()
    {
        if (CheckIfCanRest())
        {
            _ghostManager.ChangeState(_ghostManager.GetGhostIdleState());
            _canDisappear = true;
        }
        else if (CheckIfCanDisappear())
        {
            _ghostManager.ChangeState(_ghostManager.GetGhostDisappearState());
            _canDisappear = false;
        }
        else if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _ghostManager.FlippingSprite();
        }
        //base.Update();
    }

    private void HandleRandomLeftRightDirection()
    {
        _rdLeftRightDir = Random.Range(0, 2);
        if (_ghostManager.GetIsFacingRight() && _rdLeftRightDir == 0)
        {
            //Debug.Log("Random Flip Left");
            _ghostManager.FlippingSprite();
        }
        else if (!_ghostManager.GetIsFacingRight() && _rdLeftRightDir > 0)
        {
            //Debug.Log("Random Flip Right");
            _ghostManager.FlippingSprite();
        }
    }

    private bool CheckIfCanRest()
    {
        return Time.time - _entryTime >= _ghostManager.WanderTime && !_canDisappear;
    }

    private bool CheckIfCanDisappear()
    {
        return Time.time - _entryTime >= _ghostManager.WanderTime && _canDisappear;
    }

    private bool CheckIfCanChangeDirection()
    {
        if (_ghostManager.transform.position.x >= _ghostManager.RightBound.position.x && !_hasChangeDirection
            || _ghostManager.transform.position.x <= _ghostManager.LeftBound.position.x && !_hasChangeDirection)
            return true;

        return false;
    }

    public override void FixedUpdate()
    {
        if (_ghostManager.GetIsFacingRight())
            _ghostManager.GetRigidbody2D().velocity = new Vector2(_ghostManager.WanderSpeed.x, _ghostManager.WanderSpeed.y);
        else
            _ghostManager.GetRigidbody2D().velocity = new Vector2(-_ghostManager.WanderSpeed.x, _ghostManager.WanderSpeed.y);
    }
}
