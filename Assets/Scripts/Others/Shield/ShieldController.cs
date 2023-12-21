using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : GameObjectManager
{
    [Header("Time")]
    [SerializeField] private float _existTime;
    [SerializeField] private float _runningOutTime;
    private float _entryTime;

    private bool _hasTriggeredRunningOut;
    private bool _hasDisabled;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _entryTime = Time.time;
    }

    private void Update()
    {
        if (CheckIfRunningOut())
            HandleRunningOutState();
        else if (CheckIfCanDisable())
            DisableShield();
    }

    private bool CheckIfRunningOut()
    {
        return Time.time - _entryTime >= _existTime && !_hasTriggeredRunningOut;
    }

    private void HandleRunningOutState()
    {
        _hasTriggeredRunningOut = true;
        _anim.SetTrigger(GameConstants.RUNNINGOUT);
        _entryTime = Time.time;
    }

    private bool CheckIfCanDisable()
    {
        return Time.time - _entryTime >= _runningOutTime && _hasTriggeredRunningOut && !_hasDisabled;
    }

    //Vì dis nên 0 gọi lại start, nên dùng hàm này, bỏ mấy thứ ở start vào đây
    private void OnEnable()
    {
        _entryTime = Time.time;
    }

    //Bị dis thì dọn dẹp ở hàm này
    private void OnDisable()
    {
        _hasTriggeredRunningOut = false;
        _hasDisabled = false;
        _anim.SetTrigger(GameConstants.IDLE);
    }

    private void DisableShield()
    {
        _hasDisabled = true;
        this.gameObject.SetActive(false);
    }
}
