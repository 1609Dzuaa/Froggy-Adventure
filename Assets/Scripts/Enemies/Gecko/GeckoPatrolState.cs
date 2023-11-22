using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class GeckoPatrolState : GeckoBaseState
{
    private float entryTime;
    private bool hasChangeState = false;
    private int randomDirection;

    public override void EnterState(GeckoStateManager geckoStateManager)
    {
        base.EnterState(geckoStateManager);
        geckoStateManager.GetAnimator().SetInteger("state", (int)EnumState.EGeckoState.patrol);
        entryTime = Time.time;
        randomDirection = Random.Range(0, 2);
        hasChangeState = false;
        //Debug.Log("Pt");
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (Time.time - entryTime >= _geckoStateManager.GetPatrolTime())
        {
            _geckoStateManager.ChangeState(_geckoStateManager.geckoIdleState);
            hasChangeState = true;
        }
        else if (_geckoStateManager.GetHasDetectPlayer())
        {
            _geckoStateManager.ChangeState(_geckoStateManager.geckoHideState);
            hasChangeState = true;
        }
    }

    public override void FixedUpdate()
    {
        if (!hasChangeState)
        {
            if (randomDirection > 0)
            {
                if (!_geckoStateManager.GetIsFacingRight())
                    _geckoStateManager.FlippingSprite();
                _geckoStateManager.GetRigidBody2D().velocity = new Vector2(_geckoStateManager.GetPatrolSpeed(), _geckoStateManager.GetRigidBody2D().velocity.y);
            }
            else
            {
                if (_geckoStateManager.GetIsFacingRight())
                    _geckoStateManager.FlippingSprite();
                _geckoStateManager.GetRigidBody2D().velocity = new Vector2(-1 * _geckoStateManager.GetPatrolSpeed(), _geckoStateManager.GetRigidBody2D().velocity.y);
            }
        }
    }
}
