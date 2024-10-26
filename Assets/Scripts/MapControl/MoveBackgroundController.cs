using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackgroundController : MonoBehaviour
{
    [SerializeField] Transform _camera;

    [Header("hệ số mà mình muốn cái BG nó di chuyển theo Cam")]
    [SerializeField] Vector2 _relativeOffset;

    private Vector3 _lastCameraPosition;

    void Start()
    {
        //cam theo player
        //bg theo cam
        _lastCameraPosition = _camera.position;
    }

    //LateUpdate chạy sau Update 
    void LateUpdate()
    {
        Vector3 deltaMovement = _camera.position - _lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * _relativeOffset.x, deltaMovement.y * _relativeOffset.y, 0);

        _lastCameraPosition = _camera.position;
    }
}
