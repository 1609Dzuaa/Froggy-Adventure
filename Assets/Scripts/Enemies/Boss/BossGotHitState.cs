using UnityEngine;

public class BossGotHitState : MEnemiesBaseState
{
    BossStateManager _bossManager;
    int _currentHP;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.hitShieldOff);
        _currentHP = --_bossManager.MaxHP;

        //Thêm cho nó 1 cutscene lúc chết
        if (_currentHP < 0)
        {
            _bossManager.StopAllCoroutines();
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.OnLevelCompleted, null);
        }
        //Debug.Log("GH");
    }

    public override void ExitState() { _bossManager.HasGotHit = false; }

    public override void Update() { }

    public override void FixedUpdate() { }
}
