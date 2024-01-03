using UnityEngine;

public class SnailGotHitState : MEnemiesGotHitState
{
    private SnailManager _snailManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //base.EnterState(charactersManager);
        _snailManager = (SnailManager)charactersManager;
        //2 dòng dưới để vẽ đè snail lên map, giúp nó 0 bị map che khi die
        _snailManager.SpriteRenderer.sortingLayerName = GameConstants.RENDER_MAP_LAYER;
        _snailManager.SpriteRenderer.sortingOrder = 1;
        _snailManager.Animator.SetInteger("state", (int)EnumState.ESnailState.gotHit);
        _snailManager.GetRigidbody2D().velocity = Vector2.zero; //Cố định vị trí
        _snailManager.Collider2D.enabled = false;
        _snailManager.GetRigidbody2D().gravityScale = 1f;
        //Debug.Log("GH");
        //Chỉnh lại Box Trigger khi Defend
        //Có thể spawn item gì đó sau khi snail chết
        //Player hấp thụ item để dần unlock skill WallSlide
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
