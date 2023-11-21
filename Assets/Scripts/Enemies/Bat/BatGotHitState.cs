using UnityEngine;

public class BatGotHitState : BatBaseState
{
    private float Zdegree = 0f;
    private float lastRotateTime;

    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.gotHit);
        _batStateManager.GetCapsuleCollider2D().enabled = false;
        _batStateManager.GetRigidBody2D().bodyType = RigidbodyType2D.Dynamic;
        lastRotateTime = Time.time;
    }

    public override void ExitState() { }

    public override void UpdateState() 
    {
        if (Time.time - lastRotateTime >= _batStateManager.GetTimeEachRotate())
        {
            Zdegree -= _batStateManager.GetDegreeEachRotation();
            _batStateManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }

    public override void FixedUpdate() { }
}
