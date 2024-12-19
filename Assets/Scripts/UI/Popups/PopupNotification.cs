using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    UnityAction _btnContinueCallback;
    UnityAction _btnYesCallback;
    UnityAction _btnNoCallback;

    private void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.NotificationOnPopup, ReceiveInformation);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.NotificationOnPopup, ReceiveInformation);
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
        //Debug.Log("No runtime callbacks1: " + CountRuntimeListeners(_btnNo));

        if (_btnContinueCallback != null)
            _btnContinue.onClick.RemoveListener(_btnContinueCallback);
        if (_btnYesCallback != null)
            _btnYes.onClick.RemoveListener(_btnYesCallback);
        if (_btnNoCallback != null)
            _btnNo.onClick.RemoveListener(_btnNoCallback);

        if (param.BtnContinueCallback != null)
            _btnContinue.onClick.AddListener(param.BtnContinueCallback);

        if (param.BtnYesCallback != null)
            _btnYes.onClick.AddListener(param.BtnYesCallback);

        if (param.BtnNoCallback != null)
            _btnNo.onClick.AddListener(param.BtnNoCallback);

        //Debug.Log("No runtime callbacks2: " + CountRuntimeListeners(_btnNo));


        //cache callbacks
        _btnContinueCallback = param.BtnContinueCallback;
        _btnYesCallback = param.BtnYesCallback;
        _btnNoCallback = param.BtnNoCallback;

        //Debug.Log("Hello");
    }

    //gpt : đếm số listener đc thêm lúc runtime
    int CountRuntimeListeners(Button button)
    {
        // Access the private field "m_Calls" of UnityEvent
        FieldInfo field = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
        object mCalls = field.GetValue(button.onClick);

        // Get the list of runtime listeners
        FieldInfo runtimeCallsField = mCalls.GetType().GetField("m_RuntimeCalls", BindingFlags.NonPublic | BindingFlags.Instance);
        var runtimeCalls = runtimeCallsField.GetValue(mCalls) as System.Collections.IList;

        return runtimeCalls?.Count ?? 0;
    }
}
