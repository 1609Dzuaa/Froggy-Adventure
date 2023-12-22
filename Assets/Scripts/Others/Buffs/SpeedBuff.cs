using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _duration;

    private static SpeedBuff _speedBuffInstance;
    private bool _isAllowToUpdate;
    private bool _hasApplied;
    private float _entryTime;

    public static SpeedBuff Instance
    {
        get
        {
            if (_speedBuffInstance == null)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _speedBuffInstance = FindObjectOfType<SpeedBuff>();

                if (!_speedBuffInstance)
                    Debug.Log("No SpeedBuff in scene");
            }
            return _speedBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } set { _isAllowToUpdate = value; } }

    public float EntryTime { set { _entryTime = value; } }

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (!_speedBuffInstance)
        {
            _speedBuffInstance = this;
            DontDestroyOnLoad(gameObject); //Đảm bảo thằng này 0 bị huỷ khi chuyển Scene
            //Docs: Don't destroy the target Object when loading a new Scene.
        }
        else
        {
            //Nếu đã tồn tại thằng Instance != ở trong game thì destroy thằng này
            Destroy(gameObject);
        }
    }

    /*private void OnEnable()
    {
        PlayerStateManager.OnAppliedBuff += ApplyBuff;
    }

    private void OnDisable()
    {
        PlayerStateManager.OnAppliedBuff -= ApplyBuff;
    }*/

    private void Update()
    {
        if(_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _duration)
            {
                _playerStats.SPEED_X /= _speedMultiplier;
                _isAllowToUpdate = false;
                _hasApplied = false;
                Debug.Log("Timeout!");
            }
        }
    }

    public void ApplyBuff()
    {
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        if (!_hasApplied)
        {
            _hasApplied = true;
            _playerStats.SPEED_X *= _speedMultiplier;
        }
    }

}

