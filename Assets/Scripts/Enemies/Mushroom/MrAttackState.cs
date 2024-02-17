using UnityEngine;

public class MrAttackState : MEnemiesAttackState
{
    private bool _hasChangeState;
    private MushroomManager _mushroomManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mushroomManager = (MushroomManager)charactersManager;
        _mushroomManager.Invoke("AllowUpdateAttack", 0.15f);
        //Đại khái là lúc vào state này thì delay xíu để rotate theo trục y 180 độ
        //Nếu 0 có delay thì dẫn đến việc ĐK * thoả mãn ngay lập tức
        //=> chuyển mr về idle dù vẫn detected ra player đằng sau 
        _mushroomManager.FlippingSprite();
        //SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.MushroomScreamSfx, 1.0f);
        //Debug.Log("Attack");
    }

    public override void ExitState()
    {
        _allowUpdate = false;
        _hasChangeState = false;
    }

    public override void Update()
    {
        if(_allowUpdate)
        {
            if(!_mushroomManager.IsDetected && !_hasChangeState) //*
            {
                if (!_mushroomManager.HasCollidedWall)
                    _mushroomManager.MEnemiesPatrolState.CanRdDirection = true;
                else
                {
                    //Flip ở đây luôn để tạo cảm giác nó quay mặt lại nhưng 0 thấy Player đâu
                    _mushroomManager.FlippingSprite();
                    _mushroomManager.MEnemiesPatrolState.CanRdDirection = false;
                    _mushroomManager.MEnemiesPatrolState.SetHasJustHitWall(true);
                    //Debug.Log("Flip here");
                }
                _hasChangeState = true;
                _mushroomManager.ChangeState(_mushroomManager.MEnemiesIdleState);
                //Debug.Log("Idle here");
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate(); //Đã bao gồm việc gọi Attack trong đây
    }

    protected override void Attack()
    {
        //Mushroom 0 attack mà sẽ run away from player
        if(_mushroomManager.GetIsFacingRight())
            _mushroomManager.GetRigidbody2D().velocity = new Vector2(_mushroomManager.MEnemiesSO.ChaseSpeed.x, _mushroomManager.GetRigidbody2D().velocity.y);
        else
            _mushroomManager.GetRigidbody2D().velocity = new Vector2(-_mushroomManager.MEnemiesSO.ChaseSpeed.x, _mushroomManager.GetRigidbody2D().velocity.y);
    }
}
