using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisibleBuff : PlayerBuffs
{
    //Record xem lại tăng giảm alpha

    [SerializeField] Transform _playerRef;
    [SerializeField] private float _alphaApply;
    [SerializeField] private float _alphaApplyRunOut;
    [SerializeField] private float _runOutDuration;
    [SerializeField] private float _timeEachRunOutEffect;

    private SpriteRenderer _playerSpriteRenderer;

    private float _entryRunOutTime;
    private float _entryEachRunOutTime;
    private bool _hasTickRunOut;
    private bool _isDecrease;

    public override void Awake()
    {
        _playerSpriteRenderer = _playerRef.GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        if (_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _buffDuration)
            {
                StartTickRunOut();

                if(Time.time - _entryRunOutTime < _runOutDuration)
                    HandleIncreaseDecreaseAlpha();
                else
                    ResetBuffData();
            }
            /*else
                Debug.Log("PlayerColor: " + _playerSpriteRenderer.color.a);*/
        }
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        _hasTickRunOut = false; //Vì có thể runout r lại ăn buff
        _playerSpriteRenderer.color = new Color(1f, 1f, 1f, _alphaApply);
        //Debug.Log("da apply Invi");
    }

    private void StartTickRunOut()
    {
        if (!_hasTickRunOut)
        {
            _hasTickRunOut = true;
            _entryRunOutTime = Time.time;
            _entryEachRunOutTime = Time.time;
        }
    }

    private void HandleIncreaseDecreaseAlpha()
    {
        if (Time.time - _entryEachRunOutTime >= _timeEachRunOutEffect)
        {
            _entryEachRunOutTime = Time.time;
            if (!_isDecrease)
            {
                _isDecrease = true;
                _playerSpriteRenderer.color = new Color(1f, 1f, 1f, _alphaApplyRunOut);
            }
            else
            {
                _isDecrease = false;
                _playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1 - _alphaApplyRunOut);
            }
        }
    }

    private void ResetBuffData()
    {
        _isAllowToUpdate = false;
        _hasTickRunOut = false;
        _playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        //Debug.Log("Timeout!");
    }

}