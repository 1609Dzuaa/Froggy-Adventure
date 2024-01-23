using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyFanController : GameObjectManager
{
    [SerializeField, Range(0f, 700f)] Vector2 _pushBackForce;
    int _state;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _state = 0;
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.FanOnBeingDisabled, BeingDisabled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && _state == 0)
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnBeingPushedBack, _pushBackForce);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && _state == 0)
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnBeingPushedBack, _pushBackForce);
    }

    private void BeingDisabled(object obj)
    {
        _anim.SetTrigger("Off");
        _state = 1;
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.FanOnBeingDisabled, BeingDisabled);
    }
}
