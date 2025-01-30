using UnityEngine;
using static GameEnums;

public class BossTeleportState : MEnemiesBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)EBossState.teleport);
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        _bossManager.IsLastBreath = true;
        SpawnTeleVfx();
        _bossManager.transform.position = _bossManager.MiddleRoom.position;
        _bossManager.StartCoroutine(_bossManager.BackToWeak());
        SoundsManager.Instance.PlaySfx(ESoundName.BossTeleSfx, 1.0f);
        EventsManager.NotifyObservers(EEvents.BossOnDie);
    }

    private void SpawnTeleVfx()
    {
        GameObject gObj = Pool.Instance.GetObjectInPool(EPoolable.BossTeleVfx);
        gObj.SetActive(true);
        gObj.transform.position = _bossManager.transform.position;
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
