using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldBuff : MonoBehaviour
{
    [SerializeField] private Transform _shieldPos;
    [SerializeField] private float _shieldDuration;
    [SerializeField] private float _runningOutDuration;

    private Animator _anim;
    private CircleCollider2D _circleCollider2D;
    private static PlayerShieldBuff _shieldBuffInstance;
    private bool _isAllowToUpdate;
    private bool _hasTriggeredRunningOut;
    private bool _hasDisabled;
    private float _entryTime;

    public static PlayerShieldBuff Instance
    {
        get
        {
            if (!_shieldBuffInstance)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _shieldBuffInstance = FindObjectOfType<PlayerShieldBuff>();

                if (!_shieldBuffInstance)
                    Debug.Log("No Shield in scene");
            }
            return _shieldBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } }

    public float EntryTime { set { _entryTime = value; } }

    private void Awake()
    {
        CreateInstance();
        _anim = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.enabled = false;
    }

    private void CreateInstance()
    {
        if (!_shieldBuffInstance)
        {
            _shieldBuffInstance = this;
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
            transform.position = _shieldPos.position;
            if (CheckIfRunningOut())
                HandleRunningOutState();
            else if (CheckIfCanDisable())
                DisableShield();
        }
    }

    public void ApplyBuff()
    {
        //Reset lại data khi apply buff
        transform.position = _shieldPos.position;
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        _anim.SetTrigger("Idle");
        _hasTriggeredRunningOut = false;
        _hasDisabled = false;
        _circleCollider2D.enabled = true;
    }

    private bool CheckIfRunningOut()
    {
        return Time.time - _entryTime >= _shieldDuration && !_hasTriggeredRunningOut;
    }

    private void HandleRunningOutState()
    {
        _hasTriggeredRunningOut = true;
        _anim.SetTrigger(GameConstants.RUNNINGOUT);
        _entryTime = Time.time;
    }

    private bool CheckIfCanDisable()
    {
        return Time.time - _entryTime >= _runningOutDuration && _hasTriggeredRunningOut && !_hasDisabled;
    }

    private void DisableShield()
    {
        _hasDisabled = true;
        _isAllowToUpdate = false;
        _anim.SetTrigger("Disable");
        _circleCollider2D.enabled = false;
    }

}