using UnityEngine;

public class RhinoGotHitState : BaseState
{
    private bool allowUpdate = false;

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is RhinoStateManager)
        {
            rhinoStateManager.GetAnimator().SetInteger("state", (int)EnumState.ERhinoState.gotHit);
            rhinoStateManager.GetRigidBody2D().velocity = new Vector2(0f, 0f); //Cố định vị trí
            KnockUpAndLeft();
            rhinoStateManager.GetBoxCollider2D().enabled = false;
            rhinoStateManager.Invoke("SetTrueGotHitUpdate", 0.4f);
            //Debug.Log("GotHit"); 
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (allowUpdate)
        {
            //rhinoStateManager.GetBoxCollider2D().enabled = false;
            rhinoStateManager.DestroyItSelf();
        }
    }

    public override void FixedUpdate()
    {

    }

    private void KnockUpAndLeft()
    {
        rhinoStateManager.GetRigidBody2D().AddForce(rhinoStateManager.GetKnockForce());
        //Debug.Log("Knock");
    }
}
