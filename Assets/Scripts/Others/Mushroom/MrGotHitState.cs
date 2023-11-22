using UnityEngine;

public class MrGotHitState : MrBaseState
{
    private bool allowUpdate = false;

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(MrStateManager mrStateManager)
    {
        base.EnterState(mrStateManager);
        _mrStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.gotHit);
        _mrStateManager.GetRigidBody2D().velocity = new Vector2(0f, 0f); //Cố định vị trí
        KnockUpAndLeft();
        _mrStateManager.GetBoxCollider2D().enabled = false;
        _mrStateManager.Invoke("SetTrueGotHitUpdate", _mrStateManager.GetGotHitDuration());
        //Debug.Log("GotHit"); 
    }

    public override void ExitState()
    {

    }

    public override void Update()
    {
        if (allowUpdate)
        {
            _mrStateManager.DestroyItSelf();
        }
    }

    public override void FixedUpdate()
    {

    }

    private void KnockUpAndLeft()
    {
        _mrStateManager.GetRigidBody2D().AddForce(_mrStateManager.GetKnockForce());
        //Debug.Log("Knock");
    }

}
