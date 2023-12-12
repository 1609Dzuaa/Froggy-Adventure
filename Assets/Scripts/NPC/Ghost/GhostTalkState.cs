using UnityEngine;

public class GhostTalkState : CharacterBaseState
{
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger("state", (int)EnumState.EGhostState.appear);
        _ghostManager.GetRigidbody2D().velocity = Vector2.zero;
        HandleInteractWithPlayer();
        Debug.Log("Talk");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        //Đéo tiếp chuyện nữa thì trả về Idle State
        if (!_ghostManager.GetDialog().Started)
            _ghostManager.ChangeState(_ghostManager.GetGhostIdleState());
    }

    private void HandleInteractWithPlayer()
    {
        var playerScript = _ghostManager.PlayerRef.GetComponent<PlayerStateManager>();

        HandleFlippingSpriteIfSameSide(playerScript);

        HandleFlipingSpriteIfOppositeSide(playerScript);

        //Lấy và bắt đầu Thoại
        _ghostManager.GetDialog().StartDialog();
    }

    private void HandleFlippingSpriteIfSameSide(PlayerStateManager playerScript)
    {
        //TH 2 thằng cùng hướng:
        //Nếu cùng face right thì:
        //Vị trí x thằng nào lớn hơn thì flip thằng đó ngược lại
        //Cùng face left thì: 
        //Vị trí x thằng nào nhỏ hơn thì flip thằng đó ngược lại

        if (_ghostManager.GetIsFacingRight() && playerScript.GetIsFacingRight())
        {
            if (_ghostManager.transform.position.x > playerScript.transform.position.x)
                _ghostManager.FlippingSprite();
            else if (_ghostManager.transform.position.x < playerScript.transform.position.x)
                playerScript.FlippingSprite();
        }
        else if (!_ghostManager.GetIsFacingRight() && !playerScript.GetIsFacingRight())
        {
            if (_ghostManager.transform.position.x > playerScript.transform.position.x)
                playerScript.FlippingSprite();
            else if (_ghostManager.transform.position.x < playerScript.transform.position.x)
                _ghostManager.FlippingSprite();
        }
    }

    private void HandleFlipingSpriteIfOppositeSide(PlayerStateManager playerScript)
    {
        //TH 2 thằng ngược hướng:
        //Nếu NPC face right và vị trí x lớn hơn Player thì flip 2 thằng
        //Nếu NPC face left và vị trí x nhỏ hơn Player thì flip 2 thằng
        //Các TH ngược hướng còn lại thì đéo flip

        if (_ghostManager.GetIsFacingRight() && !playerScript.GetIsFacingRight())
        {
            if(_ghostManager.transform.position.x > playerScript.transform.position.x)
            {
                _ghostManager.FlippingSprite();
                playerScript.FlippingSprite();
            }
        }
        else if(!_ghostManager.GetIsFacingRight() && playerScript.GetIsFacingRight())
        {
            if (_ghostManager.transform.position.x < playerScript.transform.position.x)
            {
                _ghostManager.FlippingSprite();
                playerScript.FlippingSprite();
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
