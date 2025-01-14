using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : GameObjectManager
{
    //Để ý mấy cái tag, layer name nữa

    [Header("Force Apply")]
    [SerializeField] private float _forceApply;

    [Header("Time")]
    [SerializeField] private float _restartTime;

    private bool _hasAddForce;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasAddForce)
        {
            _hasAddForce = true;
            EventsManager.NotifyObservers(GameEnums.EEvents.PlayerOnJumpPassive, _forceApply);
            SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.TrampolineSfx, 1.0f);
            _anim.SetTrigger("Jump");
            StartCoroutine(RestartAddForceVar());
        }
    }

    private IEnumerator RestartAddForceVar()
    {
        yield return new WaitForSeconds(_restartTime);

        _hasAddForce = false;
    }

    private void SetIdleAnimation()
    {
        _anim.SetTrigger("Idle");
        //Event của Jump animation
    }

}
