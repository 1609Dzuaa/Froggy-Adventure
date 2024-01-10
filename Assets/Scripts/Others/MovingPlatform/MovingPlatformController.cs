using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] float _vX;
    [SerializeField] float _vY;
    [SerializeField] bool _isVertical;
    //2 cái vật thể dưới là mốc trái, phải của Platform -> ta sẽ 0 vẽ nó
    [SerializeField] Transform _maxPoint1;
    [SerializeField] Transform _maxPoint2;
    //1 = Left || Top
    //2 = Right || Bot
    //Muốn biến th này thành Horizontal/Vertical thì chỉnh ngoài Inspector

    void Update()
    {
        if (!_isVertical)
            HorizontalPlatformMove();
        else
            VerticalPlatformMove();
    }

    private void HorizontalPlatformMove()
    {
        if (transform.position.x <= _maxPoint1.position.x || transform.position.x >= _maxPoint2.position.x)
            _vX = -_vX;
        transform.position += new Vector3(_vX, 0, 0) * Time.deltaTime;
    }

    private void VerticalPlatformMove()
    {
        if (transform.position.y >= _maxPoint1.position.y || transform.position.y <= _maxPoint2.position.y)
            _vY = -_vY;
        transform.position += new Vector3(0, _vY, 0) * Time.deltaTime;
    }
}
