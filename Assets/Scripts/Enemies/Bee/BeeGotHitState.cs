using UnityEngine;

public class BeeGotHitState : MEnemiesGotHitState
{
    private BeeManager _beeManager;
    private float Zdegree = 0f;
    private float lastRotateTime;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _beeManager = (BeeManager)charactersManager;
        _beeManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EMEnemiesState.gotHit);
        _beeManager.GetCollider2D.enabled = false;
        _beeManager.GetRigidbody2D().gravityScale = 1f;
        lastRotateTime = Time.time;
        ApplyForce();
        //Debug.Log("GH");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (Time.time - lastRotateTime >= _beeManager.EnemiesSO.TimeEachRotate)
        {
            Zdegree -= _beeManager.EnemiesSO.DegreeEachRotation;
            _beeManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }

    protected void ApplyForce()
    {
        _beeManager.GetRigidbody2D().AddForce(_beeManager.EnemiesSO.KnockForce, ForceMode2D.Impulse);
    }
}
