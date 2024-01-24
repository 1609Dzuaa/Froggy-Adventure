﻿using UnityEngine;

public class NPCTalkState : CharacterBaseState
{
    protected NPCManagers _npcManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _npcManager = (NPCManagers)charactersManager;
        _npcManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.ENPCState.idle);
        _npcManager.GetRigidbody2D().velocity = Vector2.zero;
        HandleInteractWithPlayer(_npcManager, _npcManager.GetStartIndex());
        Debug.Log("Talk");
    }

    public override void ExitState() { }

    public override void Update()
    {
        //Debug.Log("NPC Update");
        //Đéo tiếp chuyện nữa thì trả về Idle State
        if (!_npcManager.GetDialog().Started)
        {
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnStopInteractWithNPCs, null);
            _npcManager.ChangeState(_npcManager.GetNPCIdleState());
        }
    }

    protected virtual void HandleInteractWithPlayer(NPCManagers npcManagers, int startIndex)
    {
        //Tương tự bên Player, Raycast 0 phát hiện Player thì flip ngược lại
        //(Vì chỉ xét 2 hướng trái phải)
        if (!npcManagers.HasDetectedPlayer)
            npcManagers.FlippingSprite();

        //Sau khi đã có hướng NPC thì dựa vào đó để xác định vị trí muốn Player đứng để bắt đầu hội thoại
        if (npcManagers.GetIsFacingRight())
            npcManagers.ConversationPos = new Vector2(npcManagers.transform.position.x + npcManagers.AdjustConversationRange, npcManagers.transform.parent.position.y);
        else
            npcManagers.ConversationPos = new Vector2(npcManagers.transform.position.x - npcManagers.AdjustConversationRange, npcManagers.transform.parent.position.y);

        //Gán vị trí cần di chuyển cho Player thông qua Event
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnInteractWithNPCs, npcManagers.ConversationPos);

        //Lấy và bắt đầu Thoại
        npcManagers.GetDialog().StartDialog(startIndex);
    }

    public override void FixedUpdate() { }
}