using UnityEngine;

public class NMEnemiesGotHitState : NMEnemiesBaseState
{
    protected float Zdegree = 0f;
    protected float lastRotateTime;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _nmEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ENMEnemiesState.gotHit);
        HandleBeforeDead();
        //Debug.Log("GH");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (Time.time - lastRotateTime >= _nmEnemiesManager.EnemiesSO.TimeEachRotate)
        {
            Zdegree -= _nmEnemiesManager.EnemiesSO.DegreeEachRotation;
            _nmEnemiesManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }

    protected virtual void HandleBeforeDead()
    {
        _nmEnemiesManager.GetCollider2D.enabled = false;
        lastRotateTime = Time.time;
        _nmEnemiesManager.GetRigidbody2D().AddForce(_nmEnemiesManager.EnemiesSO.KnockForce, ForceMode2D.Impulse);
        _nmEnemiesManager.GetSpriteRenderer.sortingLayerName = GameConstants.RENDER_MAP_LAYER;
        _nmEnemiesManager.GetSpriteRenderer.sortingOrder = GameConstants.RENDER_MAP_ORDER;
        SoundsManager.Instance.GetTypeOfSound(GameConstants.ENEMIES_DEAD_SOUND).Play();
    }
}
