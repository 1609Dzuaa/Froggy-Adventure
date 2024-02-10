using UnityEngine;

public class BossWeakState : MEnemiesIdleState
{
    BossStateManager _bossManager;
    bool _isFirstEnterState = true; //Để start coroutine về Normal State duy nhất 1 lần

    public bool IsFirstEnterState { get => _isFirstEnterState; set => _isFirstEnterState = value; }

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleNoShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        if(_isFirstEnterState)
        {
            _isFirstEnterState = false;
            _bossManager.StartCoroutine(_bossManager.TurnOnShield());
            Debug.Log("Weak");
        }
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() 
    {
        if (_bossManager.GetIsFacingRight())
            _bossManager.GetRigidbody2D().velocity = new Vector2(_bossManager.RetreatSpeed, 0f);
        else
            _bossManager.GetRigidbody2D().velocity = new Vector2(-_bossManager.RetreatSpeed, 0f);
    }
}
