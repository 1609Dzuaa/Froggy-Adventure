using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedBuff : MonoBehaviour
{
    //Buff này chắc dùng PS/TR để tạo effect
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _duration;
    [SerializeField] private Transform _speedBuffIcon; //sign báo hiệu vẫn còn thgian buff
    [SerializeField] private Transform _speedBuffIconPos;

    private static PlayerSpeedBuff _speedBuffInstance;
    private bool _isAllowToUpdate;
    private float _entryTime;

    public float SpeedMultiplier { get { return _speedMultiplier; } }

    public static PlayerSpeedBuff Instance
    {
        get
        {
            if (!_speedBuffInstance)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _speedBuffInstance = FindObjectOfType<PlayerSpeedBuff>();

                if (!_speedBuffInstance)
                    Debug.Log("No SpeedBuff in scene");
            }
            return _speedBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } }

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

    private void Start()
    {
        _speedBuffIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _duration)
            {
                _isAllowToUpdate = false;
                _speedBuffIcon.gameObject.SetActive(false);
                Debug.Log("Timeout!");
            }
            _speedBuffIcon.transform.position = _speedBuffIconPos.position;
        }
    }

    public void ApplyBuff()
    {
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        _speedBuffIcon.gameObject.SetActive(true);
        _speedBuffIcon.transform.position = _speedBuffIconPos.position;
    }

}

