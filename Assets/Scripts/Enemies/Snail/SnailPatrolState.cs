using UnityEngine;

public class SnailPatrolState : MEnemiesPatrolState
{
    private SnailManager _snailManager;
    private bool _canMoveVertical;

    public bool CanMoveVertical { get { return _canMoveVertical; } set { _canMoveVertical = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanMoveVertical())
        {
            _canMoveVertical = !_canMoveVertical;
            if (_canMoveVertical)
                _snailManager.GetRigidbody2D().bodyType = RigidbodyType2D.Kinematic;
            else
                _snailManager.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;
            //_snailManager.transform.rotation = Quaternion.Euler(0, 0, -90f);
            _snailManager.transform.Rotate(0, 0, -90f);
        }

        if (CheckIfCanRest())
            _snailManager.ChangeState(_snailManager.SnailIdleState);
        else if (CheckIfCanAttack())
            _snailManager.Invoke("AllowAttackPlayer", _snailManager.GetAttackDelay());
        else if (CheckIfCanChangeDirection())
        {
            _hasChangeDirection = true;
            _snailManager.FlippingSprite();
        }
    }

    private bool CheckIfCanMoveVertical()
    {
        if (_snailManager.HasCollidedWall)
            return true;
        return false;
    }

    public override void FixedUpdate()
    {
        if (!_canMoveVertical)
        {
            if (!_snailManager.GetIsFacingRight())
                _snailManager.GetRigidbody2D().velocity = new Vector2(-_snailManager.GetPatrolSpeed(), 0f);
            else
                _snailManager.GetRigidbody2D().velocity = new Vector2(_snailManager.GetPatrolSpeed(), 0f);
        }
        else
        {
            if (!_snailManager.GetIsFacingRight())
                _snailManager.GetRigidbody2D().velocity = new Vector2(0f, _snailManager.GetPatrolSpeed());
            else
                _snailManager.GetRigidbody2D().velocity = new Vector2(0f, -_snailManager.GetPatrolSpeed());
        }
    }

}
