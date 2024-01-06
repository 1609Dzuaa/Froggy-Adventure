using UnityEngine;

public class GeckoHideState : MEnemiesBaseState
{
    private GeckoManager _geckoManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _geckoManager = (GeckoManager)charactersManager;
        _geckoManager.Animator.SetInteger("state", (int)GameEnums.EGeckoState.hide);
        _geckoManager.GetCollider2D.enabled = false;
        _geckoManager.GetRigidbody2D().bodyType = RigidbodyType2D.Kinematic;
        //Thêm Spawn effect khi Hide
        //Dùng PS
    }

    public override void ExitState() 
    {
        _geckoManager.GetCollider2D.enabled = true;
        _geckoManager.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Update() { }

    public override void FixedUpdate() { }
}
