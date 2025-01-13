using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class PopupLevel : PopupController
{
    [SerializeField] Transform[] _arrPopupLevels;
    [SerializeField] LevelDetailData _levelDetailData;
    bool _canClose = true;

    private void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnPopupLevelCanToggle, OnPopupLevelCanToggle);
    }

    protected override void OnEnable()
    {
        if (!_isFirstOnEnable)
            OnOpen();
        else
        {
            _isFirstOnEnable = false;
            _arrPopupLevels[0].localScale = Vector3.zero;
            _arrPopupLevels[1].localScale = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnPopupLevelCanToggle, OnPopupLevelCanToggle);
    }

    //phục vụ việc tắt popup level và level detail của 2 button play và back
    public void ButtonOnClick(bool isBack)
    {
        if (isBack)
        {
            OnClose();
            _levelDetailData.gameObject.SetActive(false);
        }
        else
        {
            if (_canClose)
            {
                OnClose();
                _levelDetailData.gameObject.SetActive(false);
                Debug.Log("Start can close");
            }
        }
    }

    public override void OnOpen()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_arrPopupLevels[0].DOScale(Vector3.one, _duration)).SetEase(_ease);
        sequence.Append(_arrPopupLevels[1].DOScale(Vector3.one, _duration)).SetEase(_ease);
        Debug.Log("Level Open");
    }

    public override void OnClose()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_arrPopupLevels[0].DOScale(Vector3.zero, _duration)).SetEase(_ease);
        sequence.Append(_arrPopupLevels[1].DOScale(Vector3.zero, _duration)).SetEase(_ease);
        sequence.OnComplete(() => UIManager.Instance.TogglePopup(_popupName, false));
    }

    private void OnPopupLevelCanToggle(object obj)
    {
        _canClose = (bool)obj;
    }
}
