using UnityEngine;

public class MrGotHitState : BaseState
{
    private bool allowUpdate = false;

    public void SetTrueAllowUpdate() { this.allowUpdate = true; }

    public override void EnterState(BaseStateManager _baseStateManager)
    {
        if (_baseStateManager is MushroomStateManager)
        {
            mushroomStateManager.GetAnimator().SetInteger("state", (int)EnumState.EMushroomState.gotHit);
            mushroomStateManager.GetRigidBody2D().velocity = new Vector2(0f, 0f); //Cố định vị trí
            KnockUpAndLeft();
            mushroomStateManager.GetBoxCollider2D().enabled = false;
            mushroomStateManager.Invoke("SetTrueGotHitUpdate", mushroomStateManager.GetGotHitDuration());
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
            mushroomStateManager.DestroyItSelf();
        }
    }

    public override void FixedUpdate()
    {

    }

    private void KnockUpAndLeft()
    {
        mushroomStateManager.GetRigidBody2D().AddForce(mushroomStateManager.GetKnockForce());
        //Debug.Log("Knock");
    }

}
