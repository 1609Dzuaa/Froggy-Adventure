using UnityEngine;

public class BossNormalState : MEnemiesBaseState
{
    BossStateManager _bossManager;
    float _xAxisDistance;
    int _randomState;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        _bossManager.WeakState.IsFirstEnterState = true; //Reset cho Weak State
        Debug.Log("Normal");
    }

    public override void ExitState()
    {
        
    }

    public override void Update()
    {
        FlipTowardsPlayer();

        if (_bossManager.HasDetectedPlayer)
        {
            //Random 1 trong 3 skill sau khi ở state Normal
            _randomState = Random.Range(0, 3);
            switch (_randomState)
            {
                case 0:
                    _bossManager.ChangeState(_bossManager.ChargeState);
                    break;

                case 1:
                    _bossManager.ChangeState(_bossManager.SummonState);
                    break;

                case 2:
                    _bossManager.ChangeState(_bossManager.ParticleState);
                    break;
            }
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

    public override void FixedUpdate() { }
}
