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

        if (_slimeManager.HasStartConversation)
            HandleInteractWithPlayer(_slimeManager, _slimeManager.GetStartIndex() + 1);
        else
            HandleInteractWithPlayer(_slimeManager, _slimeManager.GetStartIndex());
        Debug.Log("Slime Talk");
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Debug.Log("Slime Update");
        base.Update();
    }

    public override void FixedUpdate() { }
}