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
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnPlayLevel, OnPlayLevel);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnPlayLevel, OnPlayLevel);
    }

    public override void OnClose()
    {
        if (_canClose)
        {
            base.OnClose();
            if (_levelDetailData != null)
                _levelDetailData.gameObject.SetActive(false);

        }
    }

    private void OnPlayLevel(object obj)
    {
        _canClose = (bool)obj;
    }
}
