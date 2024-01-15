using UnityEngine;

public class BunnyGotHitState : MEnemiesGotHitState
{
    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
    }

    public override void ExitState() { }

    public override void Update()
    {
        base.Update();
    }

    protected override void HandleBeforeDestroy()
    {
        //Rabb mass khá nhẹ nên giải pháp tạm thời là giảm force áp vào
        //tìm cách != sau
        _mEnemiesManager.GetSpriteRenderer.sortingLayerName = GameConstants.RENDER_MAP_LAYER;
        _mEnemiesManager.GetSpriteRenderer.sortingOrder = GameConstants.RENDER_MAP_ORDER;
        _mEnemiesManager.GetRigidbody2D().velocity = Vector2.zero;
        _mEnemiesManager.GetRigidbody2D().AddForce(_mEnemiesManager.EnemiesSO.KnockForce / 3, ForceMode2D.Impulse);
        Debug.Log("Force: " + _mEnemiesManager.EnemiesSO.KnockForce);
        _mEnemiesManager.GetCollider2D.enabled = false;
    }

    public override void FixedUpdate() { }
}
