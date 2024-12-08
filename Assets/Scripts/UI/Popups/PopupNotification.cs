using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static GameEnums;

public class NotificationParam
{
    public string Content;
    public bool ShowBtnContinue;
    public bool ShowBtnYesNo;
    public UnityAction BtnContinueCallback;
    public UnityAction BtnYesCallback;
    public UnityAction BtnNoCallback;

    public NotificationParam(string content, bool showBtnContinue, 
        UnityAction btnContinueCallback = null, 
        UnityAction btnYesCallback = null, UnityAction btnNoCallback = null)
    {
        Content = content;
        ShowBtnContinue = showBtnContinue;
        ShowBtnYesNo = !showBtnContinue;
        BtnContinueCallback = btnContinueCallback;
        BtnYesCallback = btnYesCallback;
        BtnNoCallback = btnNoCallback;
    }
}

public class PopupNotification : PopupController
{
    [SerializeField] TextMeshProUGUI _txtNotification;
    [SerializeField] Button _btnContinue;
    [SerializeField] Button _btnYes;
    [SerializeField] Button _btnNo;

    private void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.NotificationOnPopup, ReceiveInformation);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.NotificationOnPopup, ReceiveInformation);
    }

    private void ReceiveInformation(object obj)
    {
        NotificationParam param = obj as NotificationParam;

        //set active or deactive btns
        _txtNotification.text = param.Content;
        _btnContinue.gameObject.SetActive(param.ShowBtnContinue);
        _btnYes.gameObject.SetActive(param.ShowBtnYesNo);
        _btnNo.gameObject.SetActive(param.ShowBtnYesNo);

        //handle callbacks

        _btnContinue.onClick.RemoveAllListeners();
        _btnYes.onClick.RemoveAllListeners();
        _btnNo.onClick.RemoveAllListeners();

        if (param.BtnContinueCallback != null)
            _btnContinue.onClick.AddListener(param.BtnContinueCallback);
        _btnContinue.onClick.AddListener(OnClose);

        if (param.BtnYesCallback != null)
            _btnYes.onClick.AddListener(param.BtnYesCallback);
        _btnYes.onClick.AddListener(OnClose);

        if (param.BtnNoCallback != null)
            _btnNo.onClick.AddListener(param.BtnNoCallback);
        _btnNo.onClick.AddListener(OnClose);

        //Debug.Log("Hello");
    }
}
