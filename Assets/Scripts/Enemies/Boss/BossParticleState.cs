using UnityEngine;

//State này sẽ bật Particle lao về phía Player(0 nhanh = charge)
public class BossParticleState : MEnemiesAttackState
{
    BossStateManager _bossManager;
    bool _hasFlip;
    bool _isFirstEnterState = true;

    public bool IsFirstEnterState { get => _isFirstEnterState; set => _isFirstEnterState = value; }

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.idleShield);
        _bossManager.EnterBattle = true;
        _bossManager.StartCoroutine(_bossManager.Slam(1));
        if(_isFirstEnterState)
        {
            _isFirstEnterState = false;
            _bossManager.StartCoroutine(_bossManager.TurnOffParticle());
        }
        _bossManager.StartCoroutine(_bossManager.BackToNormal());
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.BossParticleSfx, 1.0f);
        Debug.Log("Particle");
    }

    public override void ExitState() { _hasFlip = false; }

    public override void Update()
    {
        if (CheckIfHitWall() && !_hasFlip)
        {
            _hasFlip = true;
            _bossManager.FlippingSprite();
        }
    }

    private bool CheckIfHitWall()
    {
        return _bossManager.HasCollidedWall;
    }

    public override void FixedUpdate()
    {
        if (_bossManager.GetIsFacingRight())
            _bossManager.GetRigidbody2D().velocity = new Vector2(_bossManager.ParticleOnSpeed, 0f);
        else
            _bossManager.GetRigidbody2D().velocity = new Vector2(-_bossManager.ParticleOnSpeed, 0f);
    }
}
