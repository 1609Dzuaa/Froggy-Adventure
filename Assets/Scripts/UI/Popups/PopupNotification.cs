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
        bool showBtnYesNo, UnityAction btnContinueCallback, 
        UnityAction btnYesCallback, UnityAction btnNoCallback)
    {
        Content = content;
        ShowBtnContinue = showBtnContinue;
        ShowBtnYesNo = showBtnYesNo;
        BtnContinueCallback = btnContinueCallback;
        BtnYesCallback = btnYesCallback;
        BtnNoCallback = btnNoCallback;
    }
}

//OnClick của 3 button đều phải có HidePopupNoti
public class PopupNotification : PopupController
{
    [SerializeField] TextMeshProUGUI _txtNotification;
    [SerializeField] Button _btnContinue;
    [SerializeField] Button _btnYes;
    [SerializeField] Button _btnNo;
    bool _canClose = true;

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
        else
            _btnContinue.onClick.AddListener(OnClose);

        if (param.BtnYesCallback != null)
            _btnYes.onClick.AddListener(param.BtnYesCallback);
        else
            _btnYes.onClick.AddListener(OnClose);

        if (param.BtnNoCallback != null)
            _btnNo.onClick.AddListener(param.BtnNoCallback);
        else
            _btnNo.onClick.AddListener(OnClose);

        //Debug.Log("Hello");
    }
}
