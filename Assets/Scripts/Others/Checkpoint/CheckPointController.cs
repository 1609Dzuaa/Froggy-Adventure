using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using static GameConstants;

public class CheckPointController : GameObjectManager
{
    private bool _checkActivated = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void HandleObjectState()
    {
        _ID = gameObject.name;

        if (!PlayerPrefs.HasKey(_ID))
        {
            PlayerPrefs.SetString(_ID, _ID);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey(ESpecialStates.Actived + _ID))
        {
            _anim.SetTrigger(CHECKPOINT_ANIM_FLAG_IDLE);
            _checkActivated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG) && !_checkActivated)
        {
            _checkActivated = true;
            _anim.SetTrigger(CHECKPOINT_ANIM_FLAG_OUT);
            SoundsManager.Instance.PlaySfx(ESoundName.CheckpointSfx, 1.0f);
            EventsManager.Instance.NotifyObservers(EEvents.PlayerOnUpdateRespawnPosition, transform.position);
            PlayerPrefs.SetString(ESpecialStates.Actived + _ID, "Activated");
        }
    }

    private void SetIdleAnimation()
    {
        _anim.SetTrigger(CHECKPOINT_ANIM_FLAG_IDLE);
        //Event của animation flag out
    }
}
