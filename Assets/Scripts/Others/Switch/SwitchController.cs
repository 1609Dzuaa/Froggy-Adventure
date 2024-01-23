using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : GameObjectManager
{
    bool _hasTriggered;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasTriggered)
        {
            _anim.SetTrigger("Hit");
            _hasTriggered = true;
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.FanOnBeingDisabled, null);
        }
    }
}
