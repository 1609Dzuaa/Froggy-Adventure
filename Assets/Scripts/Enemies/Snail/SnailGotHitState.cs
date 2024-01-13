using UnityEngine;

public class SnailGotHitState : MEnemiesGotHitState
{
    private SnailManager _snailManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        HandleBeforeDestroy();
        //Debug.Log("GH");
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }

    protected override void HandleBeforeDestroy()
    {
        _snailManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ESnailState.gotHit);
        _snailManager.GetRigidbody2D().velocity = Vector2.zero; //Cố định vị trí
        _snailManager.GetCollider2D.enabled = false;
        _snailManager.GetRigidbody2D().gravityScale = 1f;
    }
}
