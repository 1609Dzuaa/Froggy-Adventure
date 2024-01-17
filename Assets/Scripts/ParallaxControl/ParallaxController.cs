using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _relativeMoveRateX;
    [SerializeField] private float _relativeMoveRateY;
    [SerializeField] private bool _lockYMove;

    //Đối tượng càng xa (Layer càng thấp) thì move với rate gần 1
    //Tạo cảm giác như đối tượng 0 di chuyển 1 chút nào
    //Object closer appear to be movin' faster

    // Update is called once per frame
    void Update()
    {
        if (_lockYMove)
            transform.position = new Vector2(_camera.position.x * _relativeMoveRateX, transform.position.y);
        else
            transform.position = new Vector2(_camera.position.x * _relativeMoveRateX, _camera.position.y * _relativeMoveRateY);

        //Debug.Log("Cam posX, player posX: " + _camera.position.x + ", " + _playerRef.transform.position.x);
    }
}
