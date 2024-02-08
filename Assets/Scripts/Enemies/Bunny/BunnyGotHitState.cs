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
        _mEnemiesManager.GetRigidbody2D().AddForce(_mEnemiesManager.EnemiesSO.KnockForce / GameConstants.BUNNY_KNOCK_FORCE_DECREASE, ForceMode2D.Impulse);
        _mEnemiesManager.GetCollider2D.enabled = false;
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.EnemiesDeadSfx, 1.0f);
        PlayerPrefs.SetString(GameEnums.ESpecialStates.Deleted + _mEnemiesManager.ID, "deleted");
    }

    public override void FixedUpdate() { }
}
