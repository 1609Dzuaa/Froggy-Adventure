using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DealerManager : NPCManagers
{
    //Hết Text giới thiệu thì bắt đầu thoại
    //Đã xong, tìm cách chỉnh cam về player và làm Iventory cho Player, Shop cho Dealer
    //Để ý Wrap Mode trong Timeline:
    //Hold: Khi Tl kết thúc thì giữ nguyên hiện trạng
    //Loop: Y như tên, lặp lại Tl
    //None: Tl kết thúc thì quay về ban đầu (0 lặp)

    //Use Signal as communication channel between Timeline and other object in Scene
    //Timeline Signal has 3 pieces:
    //Signal Assets
    //Signal Emitter
    //Signal Receiver
    //Create Signal Emitter that will take Signal Assets
    //Receiver will listen for signal from Emitter and execute signal orders

    [Header("Text Related")]
    [SerializeField] private Text _txtOverHead;
    [SerializeField] private Transform _txtPosition;
    [SerializeField] private float _delayTxtEnable;
    [SerializeField] private float _timeEnableTxt;
    [SerializeField] private float _timeEachIncrease;
    [SerializeField] private float _alphaEachIncrease;
    [SerializeField] private float _decreaseDelay;

    [Header("Timeline Reference")]
    [SerializeField] PlayableDirector _timelineBackToPlayer;

    private DealerTalkState _dealerTalkState = new();

    #region Text Overhead Related
    private bool _mustDecrease;
    private float _entryTime;
    private float _alpha;
    #endregion

    public PlayableDirector TimelineBackToPlayer { get { return _timelineBackToPlayer; } set { _timelineBackToPlayer = value; } }

    protected override void Awake()
    {
        base.Awake();
        _txtOverHead.enabled = false;
        Color textColor = _txtOverHead.color;
        textColor.a = 0f; // Thiết lập alpha (độ trong suốt) về 0
        _txtOverHead.color = textColor;
        _txtOverHead.transform.position = _txtPosition.position;
        _alpha = 0;
    }

    protected override void Start()
    {
        base.Start();
        _npcTalkState = _dealerTalkState;
        _dialog.CantGetNextIndicatorText = true;
    }

    protected override void Update()
    {
        if (_txtOverHead.enabled && !_mustDecrease)
            IncreaseTextAlpha();

        if (_mustDecrease)
            DecreaseTextAlpha();

        //Debug.Log("Color: " + _txtOverHead.color.a);
        base.Update();
    }

    public void StartIntroduceText()
    {
        StartCoroutine(Enable());
        //Đhs để IEnumerator thì đéo xài ngoài Inspector đc @@
    }

    private IEnumerator Enable()
    {
        yield return new WaitForSeconds(_delayTxtEnable);

        _txtOverHead.enabled = true;
        _entryTime = Time.time; //Bấm giờ để tăng độ Alpha theo thgian cho Text

        StartCoroutine(DisableText());
    }

    private void IncreaseTextAlpha()
    {
        if (_alpha >= 1) 
            return;
        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _alpha += _alphaEachIncrease;
            Color textColor = _txtOverHead.color;
            textColor.a = _alpha;
            _txtOverHead.color = textColor;
            //Debug.Log("Color: " + _txtOverHead.color.a);
            _entryTime = Time.time;
        }
    }

    private void DecreaseTextAlpha()
    {
        if (_alpha <= 0)
            return;
        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _alpha -= _alphaEachIncrease;
            Color textColor = _txtOverHead.color;
            textColor.a = _alpha;
            _txtOverHead.color = textColor;
            if (_alpha <= 0f)
                ChangeState(_dealerTalkState);
            //Debug.Log("Color: " + _txtOverHead.color.a);
            _entryTime = Time.time;
        }
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(_timeEnableTxt);

        StartCoroutine(MustDecrease());
    }

    private IEnumerator MustDecrease()
    {
        yield return null;

        _mustDecrease = true;
    }

    protected override void HandleDialogAndIndicator()
    {
        if (_needTriggerIndicator)
            _dialog.ToggleIndicator(_isPlayerNearBy);

        //Nếu đã bắt đầu Thoại và chưa đến đoạn chờ thì tắt Indicator
        if (_dialog.Started && !_dialog.IsWaiting)
            _dialog.ToggleIndicator(false);

        //Thêm ĐK Player OnGround thì mới cho phép bắt đầu xl
        if (_isPlayerNearBy && Input.GetKeyDown(KeyCode.T) && _playerReference.GetIsOnGround() && _state is not NPCTalkState)
        {
            EventsManager.NotifyObservers(GameEnums.EEvents.PlayerOnInteractWithNPCs, null);
            Debug.Log("Trading");
            //Hiện shop lúc này
            //ChangeState(_dealerTalkState);
        }
    }

    protected override void UpdateConversationPosition()
    {
        if (_isFacingRight)
            _conversationPos = new Vector2(transform.position.x + _adjustConversationRange, transform.position.y);
        else
            _conversationPos = new Vector2(transform.position.x - _adjustConversationRange, transform.position.y);

        _playerReference.InteractPosition = _conversationPos;
    }

}
