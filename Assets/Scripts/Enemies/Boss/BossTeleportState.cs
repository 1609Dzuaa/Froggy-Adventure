using UnityEngine;

public class BossTeleportState : MEnemiesBaseState
{
    BossStateManager _bossManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        _bossManager = (BossStateManager)charactersManager;
        _bossManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EBossState.teleport);
        _bossManager.StopAllCoroutines();
        _bossManager.GetRigidbody2D().velocity = Vector2.zero;
        _bossManager.IsLastBreath = true;
        SpawnTeleVfx();
        _bossManager.transform.position = _bossManager.MiddleRoom.position;
        _bossManager.StartCoroutine(_bossManager.BackToWeak());
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.BossTeleSfx, 1.0f);
    }

    private void SpawnTeleVfx()
    {
        GameObject gObj = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.BossTeleVfx);
        gObj.SetActive(true);
        gObj.transform.position = _bossManager.transform.position;
    }

    public override void ExitState() { }

    public override void Update() { }

    public override void FixedUpdate() { }
}
