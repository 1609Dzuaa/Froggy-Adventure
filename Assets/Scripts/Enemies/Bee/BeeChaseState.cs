﻿using UnityEngine;

public class BeeChaseState : MEnemiesAttackState
{
    //State này sẽ chase đến vị trí "phía trên" Player để switch sang shoot Player
    private BeeManager _beeManager;
    private Vector2 _attackPos;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EMEnemiesState.patrol);
        _beeManager = (BeeManager)charactersManager;
        _attackPos = new Vector2(_beeManager.GetPlayer().position.x, _beeManager.GetPlayer().position.y + _beeManager.AdJustYAxisAttackRange());
        HandleLeftRightLogic();
        //1 khi vào chase r thì phải chase & shoot đến khi nào vượt min/max Nest thì tha
        //Hoặc khi 0 detect ra player nữa (Player tàng hình)
        if (!_beeManager.MustAttack)
        {
            _beeManager.MustAttack = true;
            //SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.BeeAngrySfx, 1.0f);
        }
        _beeManager.GetRigidbody2D().velocity = Vector2.zero;
        //Debug.Log("Bee Chase");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        HandleLeftRightLogic();

        if (CheckIfCanAttack())
            _beeManager.ChangeState(_beeManager.GetBeeAttackState());
        else if (CheckIfCanIdle())
            _beeManager.ChangeState(_beeManager.GetBeeIdleState());
        else if (CheckIfOutOfMinMaxRange())
        {
            _beeManager.GetBeePatrolState().HasChangedDirection = true;
            _beeManager.GetBeePatrolState().CanRdDirection = false;
            _beeManager.FlippingSprite();
            _beeManager.ChangeState(_beeManager.GetBeePatrolState());
            //Debug.Log("Out");
        }
        else
            Attack();
    }

    private bool CheckIfCanIdle()
    {
        return !_beeManager.HasDetectedPlayer;
    }

    private bool CheckIfCanAttack()
    {
        return Vector2.Distance(_beeManager.transform.position, _attackPos) < 0.1f;
    }

    private bool CheckIfOutOfMinMaxRange()
    {
        return _beeManager.transform.position.x >= _beeManager.BoundaryRight.position.x
             || _beeManager.transform.position.x <= _beeManager.BoundaryLeft.position.x;
    }

    public override void FixedUpdate()
    {
        //Phải định nghĩa nó 0 làm gì khác ở đây
        //0 thì nó tự động chạy func FixedUpdate của thằng base(MEnemiesAttackState)
    }

    protected override void Attack()
    {
        _attackPos = new Vector2(_beeManager.GetPlayer().position.x, _beeManager.GetPlayer().position.y + _beeManager.AdJustYAxisAttackRange());
        _beeManager.transform.position = Vector2.MoveTowards(_beeManager.transform.position, _attackPos, _beeManager.MEnemiesSO.ChaseSpeed.x * Time.deltaTime);
        //Debug.Log("Movinggg");
        //Move vật thể theo target
    }

    private void HandleLeftRightLogic()
    {
        //Vì ở state này move = MoveTowards nên cần phải set lại isFacingRight
        if (_beeManager.transform.position.x > _attackPos.x)
            _beeManager.SetIsFacingRight(false);
        else if (_beeManager.transform.position.x < _attackPos.x)
            _beeManager.SetIsFacingRight(true);
    }
}
