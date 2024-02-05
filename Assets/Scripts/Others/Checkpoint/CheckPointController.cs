using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : GameObjectManager
{
    private bool _checkActivated = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_checkActivated)
        {
            _checkActivated = true;
            _anim.SetTrigger(GameConstants.CHECKPOINT_ANIM_FLAG_OUT);
            SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.CheckpointSfx, 1.0f);
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnUpdateRespawnPosition, null);
        }
    }

    private void SetIdleAnimation()
    {
        _anim.SetTrigger(GameConstants.CHECKPOINT_ANIM_FLAG_IDLE);
        //Event của animation flag out
    }
}
