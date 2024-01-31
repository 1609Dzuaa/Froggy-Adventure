using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BaseSingleton<CameraController>
{
    private List<CinemachineVirtualCamera> _listCamera = new();
    //private CinemachineVirtualCamera _activeCamera = null;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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
        //_activeCamera = camPara;
        foreach (var cam in _listCamera)
            cam.Priority = (cam != camPara) ? 0 : 11;
        //Việc switch cam đơn giản là dựa trên Priority trong Cinamachine
        //CinemachineBrain sẽ lựa chọn Cam có Priority cao nhất để làm Main Camera ;)
    }
}
