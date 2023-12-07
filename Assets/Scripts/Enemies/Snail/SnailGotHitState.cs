using UnityEngine;

public class SnailGotHitState : MEnemiesGotHitState
{
    private SnailManager _snailManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        _snailManager.Animator.SetInteger("state", (int)EnumState.ESnailState.gotHit);
        _snailManager.GetRigidbody2D().velocity = Vector2.zero; //Cố định vị trí
        _snailManager.Collider2D.enabled = false;
        //Debug.Log("GH");
        //Chỉnh lại Box Trigger khi Defend
        //Có thể spawn item gì đó sau khi snail chết
        //Player hấp thụ item để dần unlock skill WallSlide
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
