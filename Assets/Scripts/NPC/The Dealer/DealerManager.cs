using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerManager : NPCManagers
{
    [Header("Text Related")]
    [SerializeField] Text _txtOverHead;
    [SerializeField] private Transform _txtPosition;
    [SerializeField] private float _delayTxtEnable;
    [SerializeField] private float _timeEnableTxt;
    [SerializeField] private float _timeEachIncrease;
    [SerializeField] private float _alphaEachIncrease;

    private bool _hasEnabled;
    private float _entryTime;
    private float _alpha;

    protected override void Awake()
    {
        _txtOverHead.enabled = false;
        Color textColor = _txtOverHead.color;
        textColor.a = 0f; // Thiết lập alpha (độ trong suốt) về 0
        _txtOverHead.color = textColor;
        _txtOverHead.transform.position = _txtPosition.position;
        _alpha = 0;
    }

    protected override void Start()
    {
        //base.Start();
    }

    protected override void Update()
    {
        if (CameraController.GetInstance().HasTriggered && !_hasEnabled)
            StartCoroutine(EnableTxt());

        if (_txtOverHead.enabled)
            IncreaseTextAlpha();

        Debug.Log("Color: " + _txtOverHead.color.a);
        //base.Update();
    }

    private IEnumerator EnableTxt()
    {
        yield return new WaitForSeconds(_delayTxtEnable);

        IncreaseTextAlpha();
        _txtOverHead.enabled = true;
        _hasEnabled = true;
        _entryTime = Time.time; //Bấm giờ để tăng độ Alpha theo thgian cho Text

        StartCoroutine(DisableTxt());
    }

    private void IncreaseTextAlpha()
    {
        if (_alpha >= 1) return;
        if (Time.time - _entryTime >= _timeEachIncrease)
        {
            _alpha += _alphaEachIncrease;
            Color textColor = _txtOverHead.color;
            textColor.a = _alpha; // Thiết lập alpha (độ trong suốt) về 0
            _txtOverHead.color = textColor;
            _entryTime = Time.time;
        }
    }

    private IEnumerator DisableTxt()
    {
        yield return new WaitForSeconds(_timeEnableTxt);

        _txtOverHead.enabled = false;
    }

}
