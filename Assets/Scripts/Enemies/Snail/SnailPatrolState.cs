using UnityEngine;

public class SnailPatrolState : MEnemiesPatrolState
{
    private SnailManager _snailManager;
    private bool _canMoveVertical;
    /*private bool _canMoveUp;
    private bool _hasFlip;
    float _currentDegree = 0f;*/

    public bool CanMoveVertical { get { return _canMoveVertical; } }

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
        //Tại sao xoay trục Z 1 góc âm 90 độ ?
        //Áp dụng bàn tay phải:
        //Cái -> y; Trỏ -> x; Giữa -> z
        //Ngón Cái duỗi ra cùng chiều với trục quay
        //Nắm 4 ngón kia lại vào lòng sẽ chỉ chiều (+) của trục quay
        //Tham khảo:
        //https://gamedev.stackexchange.com/questions/87612/quaternion-rotation-clockwise-or-counter-clockwise

        if (CheckIfCanRotateZAxisNegative90Degree())
        {
            _canMoveVertical = !_canMoveVertical;
            //_canMoveUp = true;
            if (_canMoveVertical)
                _snailManager.GetRigidbody2D().bodyType = RigidbodyType2D.Kinematic;
            else
                _snailManager.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;
            _snailManager.transform.Rotate(0, 0, -90f);
        }

        //Phần dưới để sau
        /*else if(CheckIfCanRotateZAxisPositive90Degree())
        {
            if(!_hasFlip)
            {
                _hasFlip = true;
                _canMoveVertical = !_canMoveVertical;
                _canMoveUp = false;
                _entryTime = Time.time;
            }
            if (Time.time - _entryTime >= _snailManager.TimeEachRotation)
            {
                _currentDegree += _snailManager.DegreeEachRotation;
                if (_currentDegree <= 90f)
                    _snailManager.transform.Rotate(0, 0, _currentDegree);
                _entryTime = Time.time;
            }
        }*/

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

    private bool CheckIfCanRotateZAxisNegative90Degree()
    {
        if (_snailManager.HasCollidedWall)
            return true;
        return false;
    }

    /*private bool CheckIfCanRotateZAxisPositive90Degree()
    {
        if(!_snailManager.HasDetectedGround)
            return true;
        return false;
    }*/

    protected override bool CheckIfCanChangeDirection()
    {
        //Với snail thì check trục y
        if (_snailManager.transform.position.y >= _snailManager.BoundaryRight.position.y && !_hasChangeDirection
            || _snailManager.transform.position.y >= _snailManager.BoundaryLeft.position.y && !_hasChangeDirection)
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
            //if(_canMoveUp)
            //{
                if (!_snailManager.GetIsFacingRight())
                    _snailManager.GetRigidbody2D().velocity = new Vector2(0f, _snailManager.GetPatrolSpeed());
                else
                    _snailManager.GetRigidbody2D().velocity = new Vector2(0f, -_snailManager.GetPatrolSpeed());
            //}
           /* else
            {
                if (!_snailManager.GetIsFacingRight())
                    _snailManager.GetRigidbody2D().velocity = new Vector2(0f, -_snailManager.GetPatrolSpeed());
                else
                    _snailManager.GetRigidbody2D().velocity = new Vector2(0f, +_snailManager.GetPatrolSpeed());
            }*/
        }
    }

}
