using UnityEngine;

public class GhostDisappearState : CharacterBaseState
{
    //Bug của ma là do kh có mối nối giữa animation Dis và App
    //Nên việc nó đơ ngay frame đầu của Dis là hoàn toàn có thể
    private GhostManager _ghostManager;
    private bool _allowToUpdate;

    public bool AllowToUpdate { set => _allowToUpdate = value; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EGhostState.disappear);
        //Debug.Log("Dis");
    }

    public override void ExitState() { _allowToUpdate = false; }

    public override void Update()
    {
        if (CheckIfCanAppear())
            _ghostManager.ChangeState(_ghostManager.GetGhostAppearState());
        //Debug.Log("im hereee");
    }

    private bool CheckIfCanAppear()
    {
        return _ghostManager.GetIsPlayerNearBy() && _allowToUpdate;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}