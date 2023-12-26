﻿using UnityEngine;

public class PlayerAbsorbBuff : MonoBehaviour
{
    [SerializeField] private float _duration;

    private static PlayerAbsorbBuff _absurbBuffInstance;
    private bool _isAllowToUpdate;
    private float _entryTime;

    public static PlayerAbsorbBuff Instance
    {
        get
        {
            if (_absurbBuffInstance == null)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _absurbBuffInstance = FindObjectOfType<PlayerAbsorbBuff>();

                if (!_absurbBuffInstance)
                    Debug.Log("No Absorb Buff in scene");
            }
            return _absurbBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } }

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (!_absurbBuffInstance)
        {
            _absurbBuffInstance = this;
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
            Debug.Log("Updateee");
            if (Time.time - _entryTime >= _duration)
            {
                _isAllowToUpdate = false;
                //Debug.Log("Timeout!");
            }
        }
    }

    public void ApplyBuff()
    {
        _isAllowToUpdate = true;
        _entryTime = Time.time;
    }
}