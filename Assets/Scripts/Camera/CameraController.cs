using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    private List<CinemachineVirtualCamera> _listCamera = new();
    private CinemachineVirtualCamera _activeCamera = null;

    public static CameraController Instance 
    {  
        get 
        {
            if (!_instance)
                FindObjectOfType<CameraController>();

            if (!_instance)
                Debug.Log("0 co Cam Controller trong Scene");

            return _instance; 
        } 
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void AddCameraToList(CinemachineVirtualCamera camPara)
    {
        _listCamera.Add(camPara);
    }

    public void RemoveCameraFromList(CinemachineVirtualCamera camPara)
    {
        _listCamera.Remove(camPara);
    }

    public void SwitchingCamera(CinemachineVirtualCamera camPara)
    {
        _activeCamera = camPara;
        foreach (var cam in _listCamera)
            cam.Priority = (cam != camPara) ? 0 : 11;
        //Việc switch cam đơn giản là dựa trên Priority trong Cinamachine
        //CinemachineBrain sẽ lựa chọn Cam có Priority cao nhất để làm Main Camera ;)
    }
}
