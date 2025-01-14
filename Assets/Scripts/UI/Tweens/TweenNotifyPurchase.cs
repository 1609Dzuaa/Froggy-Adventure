using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static GameEnums;
using TMPro;

public class TweenNotifyPurchase : MonoBehaviour
{
    [SerializeField] Ease _ease;
    [SerializeField] float _durationFade, _durationMove, _distance;
    [SerializeField] TextMeshProUGUI _txtNoti;
    float _initialYPos;
    Image _imageBG;

    private void Awake()
    {
        _initialYPos = transform.localPosition.y;
        _imageBG = GetComponent<Image>();
        EventsManager.SubcribeToAnEvent(EEvents.OnPurchaseSuccess, Notify);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventsManager.UnsubscribeToAnEvent(EEvents.OnPurchaseSuccess, Notify);
    }

    private void Notify(object obj)
    {
        gameObject.SetActive(true);
        float target = _initialYPos + _distance;
        Sequence firstSequence = DOTween.Sequence();

        firstSequence.Append(transform.DOLocalMoveY(target, _durationMove));
        firstSequence.Join(_imageBG.DOFade(0.45f, _durationFade));
        firstSequence.Join(_txtNoti.DOFade(1.0f, _durationFade));
        firstSequence.Play();

        firstSequence.OnComplete(() =>
        {
            Sequence secondSequence = DOTween.Sequence();

            secondSequence.Append(transform.DOLocalMoveY(target + _distance, _durationMove));
            secondSequence.Join(_imageBG.DOFade(0.0f, _durationFade));
            firstSequence.Join(_txtNoti.DOFade(0.0f, _durationFade));
            secondSequence.Play();

            secondSequence.OnComplete(() => transform.localPosition = new Vector3(transform.localPosition.x, _initialYPos, transform.localPosition.z));
        });
    }
}
