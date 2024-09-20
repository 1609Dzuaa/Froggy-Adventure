using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameEnums;

public class PopupController : MonoBehaviour
{
    [SerializeField] protected float _duration;
    [SerializeField] protected Ease _ease;
    [SerializeField] protected EPopup _popupName;
    protected bool _isFirstOnEnable = true;

    protected virtual void OnEnable()
    {
        if (!_isFirstOnEnable)
            transform.DOScale(Vector3.one, _duration).SetEase(_ease).SetUpdate(true);
        else
        {
            _isFirstOnEnable = false;
            transform.localScale = Vector3.zero;
        }
        //Debug.Log("Here: " + name);
    }

    protected virtual void OnDisable()
    {
        //Debug.Log("dis Here");
        transform.localScale = Vector3.zero;
    }

    public virtual void OnClose()
    {
        transform.DOScale(Vector3.zero, _duration).SetEase(_ease).OnComplete(() => { UIManager.Instance.TogglePopup(_popupName, false); }).SetUpdate(true);
    }
}
