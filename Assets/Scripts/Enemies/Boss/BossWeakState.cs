using UnityEngine;

public class BossWeakState : MEnemiesBaseState
{
    BossStateManager _bossManager;
    bool _isFirstEnterState = true; //Để start coroutine về Normal State duy nhất 1 lần
    float _xAxisDistance = 0;

    public bool IsFirstEnterState { get => _isFirstEnterState; set => _isFirstEnterState = value; }

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleNoShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        if (_isFirstEnterState && !_bossManager.IsLastBreath)
        {
            _isFirstEnterState = false;
            _bossManager.StartCoroutine(_bossManager.TurnOnShield());
            Debug.Log("Weak");
        }
        else if (_bossManager.IsLastBreath)
        {
            FlipTowardsPlayer();
            _bossManager.BossDialog.StartDialog(0);
            _bossManager.StartCoroutine(_bossManager.Dead());
        }
    }

    private void FlipTowardsPlayer()
    {
        _xAxisDistance = _bossManager.transform.position.x - _bossManager.PlayerRef.position.x;
        if (_xAxisDistance < 0 && !_bossManager.GetIsFacingRight() && Mathf.Abs(_xAxisDistance) >= GameConstants.BOSS_FLIPABLE_RANGE)
            _bossManager.FlipRight();
        else if (_xAxisDistance > 0 && _bossManager.GetIsFacingRight() && Mathf.Abs(_xAxisDistance) >= GameConstants.BOSS_FLIPABLE_RANGE)
            _bossManager.FlipLeft();
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() 
    {
        if (_bossManager.IsLastBreath) return;
        if (_bossManager.GetIsFacingRight())
            _bossManager.GetRigidbody2D().velocity = new Vector2(_bossManager.RetreatSpeed, 0f);
        else
            _bossManager.GetRigidbody2D().velocity = new Vector2(-_bossManager.RetreatSpeed, 0f);
    }
}
