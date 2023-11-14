using UnityEngine;

public class MrIdleState : BaseState
{
    bool hasChangeState = false; //Đảm bảo chỉ change state 1 lần duy nhất tránh việc
    //frame trước đã change sang run nhưng frame lại lại change sang Walk
    bool canRdDirection = false; //Check nếu Hitwall khi Walk thì 0 cho random hướng mà phải Walk hướng ngược lại

    public void SetCanRdDirection(bool para) { this.canRdDirection = para; }

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is MushroomStateManager)
        {
            mushroomStateManager = (MushroomStateManager)_baseStateManager;
            mushroomStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.idle);
            hasChangeState = false;
            mushroomStateManager.SetChangeRightDirection(Random.Range(0, 2));
            mushroomStateManager.GetRigidBody2D().velocity = new Vector2(0f, 0f); //Khiến nó dừng hoàn toàn, kh bị di chuyển thêm 1 đoạn ngắn
            //Debug.Log("Idle"); //Keep this, use for debugging change state
        }
    }

    public override void ExitState()
    {
        canRdDirection = false; 
        //Set như này để đảm bảo chỉ có TH thoả mãn mới random Direction đc
        //Kh đụng tường và hết thgian walk
    }

    public override void UpdateState()
    {
        if (mushroomStateManager.GetHasDetectedPlayer() && !hasChangeState)
        {
            hasChangeState = true;
            mushroomStateManager.Invoke("AllowRunFromPlayer", mushroomStateManager.GetRunDelay()); //Delay 0.3s
        }
        else if (mushroomStateManager.GetHasCollidedWall() && !hasChangeState)
        {
            hasChangeState = true;
            mushroomStateManager.FlippingSprite();
            mushroomStateManager.Invoke("AllowWalk2", mushroomStateManager.GetRestDuration());
            //Thêm ĐK này vì vẫn có thể có TH hết thgian walk nhưng lại đụng tường
            //xảy ra tình trạng vẫn có thể random hướng walk 
        }
        else if (!hasChangeState && canRdDirection)
        {
            //Nếu 0 detect ra player và 0 đụng tường thì Walk sau restDur (s)
            //và random true false để đổi hướng Walk tiếp
            hasChangeState = true;
            mushroomStateManager.Invoke("AllowWalk1", mushroomStateManager.GetRestDuration());
        }
        else if (!hasChangeState) //Can't Random Direction to Walk
        {
            //Walk theo hướng mặt (0 random)
            hasChangeState = true;
            mushroomStateManager.Invoke("AllowWalk2", mushroomStateManager.GetRestDuration());
        }
    }

    public override void FixedUpdate()
    {

    }
}
