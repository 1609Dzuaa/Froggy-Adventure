using UnityEngine;

public class BatFlyBackState : MEnemiesBaseState
{
    private BatManager _batManager;
    private bool _hasFlip;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _mEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBatState.patrol);
        //Debug.Log("Fly back");
    }

    public override void ExitState() { _hasFlip = false; }

    public override void Update()
    {
        if (CheckIfCanAttack())
            _batManager.ChangeState(_batManager.BatAttackState);
        else
            HandleFlyBackToSleepPos();
    }

    public override void FixedUpdate() { }

    private bool CheckIfCanAttack()
    {
        return _batManager.HasDetectedPlayer;
    }

    private void HandleFlyBackToSleepPos()
    {
        if (!_hasFlip)
            HandleFlipSprite();

        _batManager.transform.position = Vector2.MoveTowards(_batManager.transform.position, _batManager.SleepPos.position, _batManager.MEnemiesSO.PatrolSpeed.x * Time.deltaTime);
        if (CheckCanCeilIn())
            _batManager.ChangeState(_batManager.BatCeilInState);
        //Xử lý việc bay về chỗ ngủ và chuyển trạng thái Ceil In
    }

    private void HandleFlipSprite()
    {
        _hasFlip = true;

        if (_batManager.transform.position.x > _batManager.SleepPos.position.x)
            _batManager.FlipLeft();
        else if (_batManager.transform.position.x < _batManager.SleepPos.position.x)
            _batManager.FlipRight();
        //Xử lý lật sprite trước khi bay về chỗ ngủ
    }

    private bool CheckCanCeilIn()
    {
        return Vector2.Distance(_batManager.transform.position, _batManager.SleepPos.position) < 0.01f;
    }
}
