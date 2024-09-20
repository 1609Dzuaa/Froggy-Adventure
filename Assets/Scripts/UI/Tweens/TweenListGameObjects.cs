using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TweenListGameObjects : MonoBehaviour
{
    [SerializeField] Image[] _arrGObj;
    [SerializeField] float _duration;
    [SerializeField] float _distance;
    [SerializeField] Ease _ease;
    float _initPosition;
    float _endPosition;

    private void Awake()
    {
        _initPosition = _arrGObj[0].transform.localPosition.x;
        _endPosition = _arrGObj[0].transform.localPosition.x + _distance;
        ResetFade();
    }

    private void OnEnable()
    {
        //Debug.Log("tween!");
        //Tween(true);
        //StartCoroutine(Tween(true));
    }

    private void OnDisable()
    {
        //Tween(false);
        //StartCoroutine(Tween(false));
        //ResetFade();
    }

    private void OnDestroy()
    {
        //Debug.Log("Des");
    }

    private void ResetFade()
    {
        for (int i = 0; i < _arrGObj.Length; i++)
        {
            _arrGObj[i].DOFade(0.15f, 0f);
        }
    }

    public void Tween(bool isForward)
    {
        Sequence sequence = DOTween.Sequence();
        if (isForward)
        {
            for (int i = 0; i < _arrGObj.Length; i++)
            {
                sequence.Append(_arrGObj[i].transform
                    .DOLocalMoveX(_endPosition, _duration))
                    .SetEase(_ease);
                sequence.Append(_arrGObj[i].DOFade(1f, _duration));
                //Debug.Log("x: " + _arrGObj[i].transform.localPosition.x);
            }
        }
        else
        {
            for (int i = _arrGObj.Length - 1; i >= 0; i--)
            {
                sequence.Append(_arrGObj[i].transform
                    .DOLocalMoveX(_initPosition, _duration))
                    .SetEase(_ease);
                sequence.Append(_arrGObj[i].DOFade(1f, _duration));
                sequence.OnComplete(ResetFade);
            }
        }
    }

    /*private IEnumerator Tween(bool isForward)
    {
        yield return null;

        //Thử tween hiệu ứng mới hơn:
        //từ trái sang phải
        //từ dưới lên trên
        //...
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < _arrGObj.Length; i++)
        {
            sequence.Append(_arrGObj[i].transform
                .DOLocalMoveX(_arrGObj[i].transform.localPosition.x + 
                ((isForward) ? _distance : -_distance), _duration))
                .SetEase(_ease);
            sequence.Append(_arrGObj[i].DOFade(1f, _duration));
            //sequence.Append(_arrGObj[i].transform.DOScale(Vector3.one, _duration).SetEase(_ease));
        }

        //Debug.Log("Tween");
    }*/
}
