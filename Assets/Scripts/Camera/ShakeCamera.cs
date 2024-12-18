using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    CinemachineVirtualCamera _cam;
    [SerializeField] float _shakeIntensity;
    [SerializeField] float _shakeTime;

    float _timer;
    CinemachineBasicMultiChannelPerlin _cbmp;
    
    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _cbmp = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.CameraOnShake, ShakeCameraa);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(GameEnums.EEvents.CameraOnShake, ShakeCameraa);
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCameraa(object obj)
    {
        _cbmp.m_AmplitudeGain = _shakeIntensity;
        _timer = _shakeTime;
    }

    void StopShake()
    {
        _cbmp.m_AmplitudeGain = 0;
        _timer = 0;
    }

    private void Update()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
                StopShake();
        }
    }
}
