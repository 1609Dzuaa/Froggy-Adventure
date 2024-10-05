using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class PopupLevel : PopupController
{
    [SerializeField] LevelDetailData _levelDetailData;
    bool _canClose = true;

    private void Awake()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnPopupLevelCanToggle, OnPopupLevelCanToggle);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnPopupLevelCanToggle, OnPopupLevelCanToggle);
    }

    //phục vụ việc tắt popup level và level detail của 2 button play và back
    public void ButtonOnClick(bool isBack)
    {
        if (isBack)
        {
            base.OnClose();
            if (_levelDetailData != null)
                _levelDetailData.gameObject.SetActive(false);
        }
        else
        {
            if (_canClose)
            {
                base.OnClose();
                if (_levelDetailData != null)
                    _levelDetailData.gameObject.SetActive(false);

            }
        }
    }

    /*public override void OnClose()
    {
        if (_canClose)
        {
            base.OnClose();
            if (_levelDetailData != null)
                _levelDetailData.gameObject.SetActive(false);

        }
    }*/

    private void OnPopupLevelCanToggle(object obj)
    {
        _canClose = (bool)obj;
    }
}
