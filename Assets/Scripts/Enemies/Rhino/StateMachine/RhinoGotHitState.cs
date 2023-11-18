using UnityEngine;

public class RhinoGotHitState : RhinoBaseState
{
    private bool allowUpdate = false;

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(RhinoStateManager rhinoStateManager)
    {
        base.EnterState(rhinoStateManager);
        _rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.gotHit);
        _rhinoStateManager.GetRigidBody2D().velocity = new Vector2(0f, 0f); //Cố định vị trí
        KnockUpAndLeft();
        _rhinoStateManager.GetBoxCollider2D().enabled = false;
        _rhinoStateManager.Invoke("SetTrueGotHitUpdate", 0.4f);
        //Debug.Log("GotHit"); 
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
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
}
