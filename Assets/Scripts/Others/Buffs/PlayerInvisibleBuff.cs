using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisibleBuff : MonoBehaviour
{
    [SerializeField] Transform _playerRef;
    [SerializeField] private float _alphaApply;
    [SerializeField] private float _alphaApplyRunOut;
    [SerializeField] private float _duration;
    [SerializeField] private float _runOutDuration;
    [SerializeField] private float _timeEachRunOutEffect;

    private static PlayerInvisibleBuff _invisibleBuffInstance;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _isAllowToUpdate;
    private float _entryTime;

    private float _entryRunOutTime;
    private float _entryEachRunOutTime;
    private bool _hasTickRunOut;
    private bool _isDecrease;

    public static PlayerInvisibleBuff Instance
    {
        get
        {
            if (!_invisibleBuffInstance)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _invisibleBuffInstance = FindObjectOfType<PlayerInvisibleBuff>();

                if (!_invisibleBuffInstance)
                    Debug.Log("No InvisibleBuff in scene");
            }
            return _invisibleBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } }

    private void Awake()
    {
        CreateInstance();
        _playerSpriteRenderer = _playerRef.GetComponent<SpriteRenderer>();
    }

    private void CreateInstance()
    {
        if (!_invisibleBuffInstance)
        {
            _invisibleBuffInstance = this;
            DontDestroyOnLoad(gameObject); //Đảm bảo thằng này 0 bị huỷ khi chuyển Scene
            //Docs: Don't destroy the target Object when loading a new Scene.
        }
        else
        {
            //Nếu đã tồn tại thằng Instance != ở trong game thì destroy thằng này
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _duration)
            {
                StartTickRunOut();

                if(Time.time - _entryRunOutTime < _runOutDuration)
                    HandleIncreaseDecreaseAlpha();
                else
                    ResetBuffData();
            }
            else
                Debug.Log("PlayerColor: " + _playerSpriteRenderer.color.a);
        }
    }

    public void ApplyBuff()
    {
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        _hasTickRunOut = false; //Vì có thể runout r lại ăn buff
        _playerSpriteRenderer.color = new Color(1f, 1f, 1f, _alphaApply);
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