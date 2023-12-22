using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBuff : MonoBehaviour
{
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] private float _jumpMultiplier;
    [SerializeField] private float _duration;

    private static JumpBuff _jumpBuffInstance;
    private bool _isAllowToUpdate;
    private bool _hasApplied;
    private float _entryTime;

    public static JumpBuff Instance
    {
        get
        {
            if (_jumpBuffInstance == null)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _jumpBuffInstance = FindObjectOfType<JumpBuff>();

                if (!_jumpBuffInstance)
                    Debug.Log("No JumpBuff in scene");
            }
            return _jumpBuffInstance;
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
        if (!_jumpBuffInstance)
        {
            _jumpBuffInstance = this;
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
                _playerStats.SPEED_Y /= _jumpMultiplier;
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
            _playerStats.SPEED_Y *= _jumpMultiplier;
        }
    }

}