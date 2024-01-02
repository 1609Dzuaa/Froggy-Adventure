using UnityEngine;

public class SnailPatrolState : MEnemiesPatrolState
{
    private SnailManager _snailManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        _snailManager.Animator.SetInteger("state", (int)EnumState.EMEnemiesState.patrol);
        _entryTime = Time.time;
        //Debug.Log("Patrol, canRD, hasChangeDir, HW: " + _canRdDirection + ", " + _hasChangeDirection + ", " + _hasJustHitWall);
        //if (_canRdDirection)
            //HandleRandomChangeDirection();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        //Bàn tay phải:
        //Cái -> y; Trỏ -> x; Giữa -> z
        //Ngón Cái duỗi ra cùng chiều với trục quay
        //Nắm 4 ngón kia lại vào lòng sẽ chỉ chiều (+) của trục quay
        //Tham khảo:
        //https://gamedev.stackexchange.com/questions/87612/quaternion-rotation-clockwise-or-counter-clockwise

        if (CheckIfCanAttack())
            _snailManager.Invoke("AllowAttackPlayer", _snailManager.GetAttackDelay());
    }

    public override void FixedUpdate()
    {
        if (!_snailManager.IsMovingVertical) //move ngang
        {
            //trong thgian rotate
            if (_snailManager.HasRotate && !_snailManager.DoneRotate)
            {
                if (_snailManager.Direction == 1)
                {
                    _snailManager.GetRigidbody2D().velocity = new Vector2(-_snailManager.GetPatrolSpeed().x, -_snailManager.GetPatrolSpeed().y);
                    //Debug.Log("trai xuong");
                }
                else if (_snailManager.Direction == 3)
                {
                    _snailManager.GetRigidbody2D().velocity = new Vector2(_snailManager.GetPatrolSpeed().x, _snailManager.GetPatrolSpeed().y);
                    //Debug.Log("phai len");
                }
            }
            else //move ngang bthg
            {
                switch (_snailManager.Direction)
                {
                    case 1:
                        _snailManager.GetRigidbody2D().velocity = new Vector2(-_snailManager.GetPatrolSpeed().x, 0f);
                        //Debug.Log("ngang1");
                        break;
                    case 3:
                        _snailManager.GetRigidbody2D().velocity = new Vector2(_snailManager.GetPatrolSpeed().x, 0f);
                        //Debug.Log("ngang3");
                        break;
                }
            }
        }
        else //move doc
        {
            //trong thgian rotate
            if(_snailManager.HasRotate && !_snailManager.DoneRotate)
            {
                if (_snailManager.Direction == 2)
                {
                    _snailManager.GetRigidbody2D().velocity = new Vector2(_snailManager.GetPatrolSpeed().x, - _snailManager.GetPatrolSpeed().y);
                    //Debug.Log("trai len2");
                }
                else if (_snailManager.Direction == 4)
                {
                    _snailManager.GetRigidbody2D().velocity = new Vector2(-_snailManager.GetPatrolSpeed().x, _snailManager.GetPatrolSpeed().y);
                    //Debug.Log("phai len");
                }
            }
            else //move doc bthg
            {
                switch (_snailManager.Direction)
                {
                    case 2:
                        _snailManager.GetRigidbody2D().velocity = new Vector2(0f, -_snailManager.GetPatrolSpeed().x);
                        //Debug.Log("doc2");
                        break;
                    case 4:
                        _snailManager.GetRigidbody2D().velocity = new Vector2(0f, _snailManager.GetPatrolSpeed().x);
                        //Debug.Log("doc4");
                        break;
                }
            }
        }
    }

    protected override bool CheckIfCanAttack()
    {
        return _snailManager.HasDetectedPlayer && !_hasChangedState;
    }

}
