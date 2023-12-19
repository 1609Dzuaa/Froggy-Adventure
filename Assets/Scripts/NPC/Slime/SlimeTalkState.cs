using UnityEngine;

public class SlimeTalkState : NPCTalkState
{
    private SlimeManager _slimeManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //_npcManager = (NPCManagers)charactersManager;
        _slimeManager = (SlimeManager)charactersManager;
        _npcManager = _slimeManager;
        _slimeManager.Animator.SetInteger("state", (int)EnumState.ESlimState.idle);
        _slimeManager.GetRigidbody2D().velocity = Vector2.zero;

        //Xử lý nếu SC bị động thì lấy IndexIfGH
        //Còn 0 thì lấy StartIndex bthg
        if (_slimeManager.HasStartConversationPassive)
            HandleInteractWithPlayer(_slimeManager, _slimeManager.GetStartIndexIfGotHit());
        else
            HandleInteractWithPlayer(_slimeManager, _slimeManager.GetStartIndex());
        Debug.Log("Slime Talk");
    }

    public override void ExitState() 
    { 
        base.ExitState();
        _slimeManager.HasStartConversationPassive = false; //Reset cho lần sau
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate() { }
}