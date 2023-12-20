using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DealerManager : NPCManagers
{
    //Hết Text giới thiệu thì bắt đầu thoại

    [Header("Text Related")]
    [SerializeField] private Text _txtOverHead;
    [SerializeField] private Transform _txtPosition;
    [SerializeField] private float _delayTxtEnable;
    [SerializeField] private float _timeEnableTxt;
    [SerializeField] private float _timeEachIncrease;
    [SerializeField] private float _alphaEachIncrease;

    private DealerTalkState _dealerTalkState = new();

    private bool _hasEnabled;
    private bool _hasDisabled;
    private float _entryTime;
    private float _alpha;

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
    }

    protected override void Update()
    {
        if (CameraController.GetInstance().HasTriggered && !_hasEnabled)
            StartCoroutine(EnableText());

        if (_txtOverHead.enabled)
            IncreaseTextAlpha();

        //Xem lại tăng giảm alpha lúc mượt lúc 0
        //Debug.Log("Color: " + _txtOverHead.color.a);
        base.Update();
    }

    private IEnumerator EnableText()
    {
        yield return new WaitForSeconds(_delayTxtEnable);

        //IncreaseTextAlpha();
        _txtOverHead.enabled = true;
        _hasEnabled = true;
        _entryTime = Time.time; //Bấm giờ để tăng độ Alpha theo thgian cho Text

        if (!_hasDisabled)
            StartCoroutine(DisableText());
    }

    private void IncreaseTextAlpha()
    {
        //_hasIncreased = true;
        if (_alpha >= 1) 
            return;
        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _alpha += _alphaEachIncrease;
            Color textColor = _txtOverHead.color;
            textColor.a = _alpha; // Thiết lập alpha (độ trong suốt) về 0
            _txtOverHead.color = textColor;
            Debug.Log("Color: " + _txtOverHead.color.a);
            _entryTime = Time.time;
        }
    }

    private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(_timeEnableTxt);

        _txtOverHead.enabled = false;
        _hasDisabled = true;    

        ChangeState(_dealerTalkState);
       // _dialog.StartDialog(_startIndex);
        //_dialog.StartConversationPassive = true;
    }

    /*public void LookAtMe()
    {
        CameraController.GetInstance().SetTargetToFollow = transform;
    }*/

}
