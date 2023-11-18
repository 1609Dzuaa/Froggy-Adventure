﻿using UnityEngine;

public class MrIdleState : MrBaseState
{
    private bool hasDetectedPlayer = false; //Gác cổng của Run, đảm bảo
    //chỉ chang state 1 lần duy nhất và ưu tiên change sang Run ở state này
    //tránh việc cờ hasChangeState đã đc đánh nên 0 change state Run đc
    private bool hasChangeState = false; //Đảm bảo chỉ change state 1 lần duy nhất tránh việc
    //frame trước đã change sang run nhưng frame lại lại change sang Walk
    private bool canRdDirection = false; //Check nếu Hitwall khi Walk thì 0 cho random hướng mà phải Walk hướng ngược lại

    public void SetCanRdDirection(bool para) { this.canRdDirection = para; }

    public override void EnterState(MrStateManager mrStateManager)
    {
        base.EnterState(mrStateManager);
        _mrStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.idle);
        _mrStateManager.SetChangeRightDirection(Random.Range(0, 2));
        _mrStateManager.GetRigidBody2D().velocity = new Vector2(0f, 0f); //Khiến nó dừng hoàn toàn, kh bị di chuyển thêm 1 đoạn ngắn
        //Debug.Log("Idle"); //Keep this, use for debugging change state
    }

    public override void ExitState()
    {
        hasDetectedPlayer = false;
        hasChangeState = false;
        canRdDirection = false;
        //Set như này để đảm bảo chỉ có TH thoả mãn mới random Direction đc
        //Kh đụng tường và hết thgian walk
    }

    public override void UpdateState()
    {
        //Lạm dụng Invoke khiến Update 0 đc thực thi làm cho mushroom cứng đơ
        //trong khoảng thgian chờ sau khi Invoke đc gọi
        if (_mrStateManager.GetHasDetectedPlayer() && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true; //Bật cờ
            hasChangeState = true; //ĐK để chuyển sang Run thì 0 quan tâm thg này lắm
            //Bật nó để báo cho mấy đk dưới 0 đc phép chuyển nữa
            //Xoá các hàm đc Invoke trc đó, ưu tiên state Run
            _mrStateManager.CancelInvoke();
            _mrStateManager.Invoke("AllowRunFromPlayer", _mrStateManager.GetRunDelay());
        }
        /*else if (MrStateManager.GetHasCollidedWall() && !hasChangeState)
        {
            //Need to check again !
            //hasChangeState = true;
            MrStateManager.FlippingSprite();
            MrStateManager.Invoke("AllowWalk2", MrStateManager.GetRestDuration());
            //Thêm ĐK này vì vẫn có thể có TH hết thgian walk nhưng lại đụng tường
            //xảy ra tình trạng vẫn có thể random hướng walk 
        }*/
        else if (!hasChangeState && canRdDirection)
        {
            //Nếu 0 detect ra player và 0 đụng tường thì Walk sau restDur (s)
            //và random true false để đổi hướng Walk tiếp
            hasChangeState = true;
            _mrStateManager.Invoke("AllowWalk1", _mrStateManager.GetRestDuration());
        }
        else if (!hasChangeState) //Can't Random Direction to Walk
        {
            //Walk theo hướng mặt (0 random)
            hasChangeState = true;
            _mrStateManager.Invoke("AllowWalk2", _mrStateManager.GetRestDuration());
        }
    }

    public override void FixedUpdate()
    {

    }
}
