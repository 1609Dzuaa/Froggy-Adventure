using UnityEngine;

public class RhinoGotHitState : RhinoBaseState
{
    private bool allowUpdate = false; //Delay trễ xíu để chạy animation

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.gotHit);
        HandleBeforeDestroy();
        //Debug.Log("GotHit"); 
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        if (allowUpdate)
        {
            //rhinoStateManager.GetBoxCollider2D().enabled = false;
            _rhinoStateManager.DestroyItSelf();
        }
    }

    public override void FixedUpdate()
    {

    }

    private void KnockUpAndLeft()
    {
        _rhinoStateManager.GetRigidBody2D().AddForce(_rhinoStateManager.GetKnockForce());
        //Debug.Log("Knock");
    }

    protected void HandleBeforeDestroy()
    {
        _rhinoStateManager.GetRigidBody2D().velocity = Vector2.zero; //Cố định vị trí
        KnockUpAndLeft();
        _rhinoStateManager.GetBoxCollider2D().enabled = false;
    }
}
