using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PushBackInfor
{
    private Vector2 _pushForce;
    private bool _isPushFromRight;

    public PushBackInfor(Vector2 pushForce, bool isPushFromRight) { _pushForce = pushForce; _isPushFromRight = isPushFromRight; }

    public Vector2 PushForce { get => _pushForce; }

    public bool IsPushFromRight { get => _isPushFromRight; }
}

public class OnlyFanController : GameObjectManager
{
    [SerializeField, Range(0f, 700f)] Vector2 _pushBackForce;
    int _state;
    bool _isFacingRight;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        base.SetUpProperties();
        _isFacingRight = (transform.eulerAngles.z) >= 90f && (transform.eulerAngles.z) < 270f;
        _state = 0;
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.FanOnBeingDisabled, BeingDisabled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && _state == 0)
        {
            PushBackInfor pInfo = new PushBackInfor(_pushBackForce, _isFacingRight);
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnBeingPushedBack, pInfo);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && _state == 0)
        {
            PushBackInfor pInfo = new PushBackInfor(_pushBackForce, _isFacingRight);
            EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnBeingPushedBack, pInfo);
        }
    }

    private void BeingDisabled(object obj)
    {
        //Chú ý ở đây kh check vì muốn khi bấm SW thì vô hiệu hoá mọi Fan hiện có

        //Có bug bị null ở đây
        if (!_anim)
            Debug.Log("Fan's anim NULL");
        else
            _anim.SetTrigger("Off");
        _state = 1;
        //EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.FanOnBeingDisabled, BeingDisabled);
    }
}
