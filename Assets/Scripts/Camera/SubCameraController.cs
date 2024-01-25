using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraController : MonoBehaviour
{
    //Các cam phụ gán script này vào để đc thêm vào List Camera để dễ quản lý

    private void Start()
    {
        CameraController.Instance.AddCameraToList(GetComponent<CinemachineVirtualCamera>());
    }

    private void OnDisable()
    {
        CameraController.Instance.RemoveCameraFromList(GetComponent<CinemachineVirtualCamera>());
    }
}
