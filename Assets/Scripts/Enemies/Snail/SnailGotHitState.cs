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
        //2 dòng dưới để vẽ đè snail lên map, giúp nó 0 bị map che khi die
        _snailManager.SpriteRenderer.sortingLayerName = GameConstants.RENDER_MAP_LAYER;
        _snailManager.SpriteRenderer.sortingOrder = 1;
        _snailManager.Animator.SetInteger("state", (int)GameEnums.ESnailState.gotHit);
        _snailManager.GetRigidbody2D().velocity = Vector2.zero; //Cố định vị trí
        _snailManager.GetCollider2D.enabled = false;
        _snailManager.GetRigidbody2D().gravityScale = 1f;
    }
}
