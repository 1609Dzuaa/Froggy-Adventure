using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectController : GameObjectManager
{
    [SerializeField] protected float _speed;
    [SerializeField] protected bool _isVertical;
    [SerializeField] protected Transform _maxPoint1;
    [SerializeField] protected Transform _maxPoint2;
    [SerializeField, Tooltip("Nếu cần active false sau ... (s)")] protected float _existTime;
    protected bool _needMinMax = true;
    protected float _entryTime;
    //1 = Left || Top
    //2 = Right || Bot
    //Muốn biến th này thành Horizontal/Vertical thì chỉnh ngoài Inspector

    public float Speed { get => _speed; set => _speed = value; }

    public bool NeedMinMax { get => _needMinMax; set => _needMinMax = value; }

    private void OnEnable()
    {
        _entryTime = Time.time;
    }

    protected virtual void Update()
    {
        if (!_isVertical)
            HorizontalMove();
        else
            VerticalMove();
    }

    private void HorizontalMove()
    {
        if (_needMinMax)
        {
            if (transform.position.x <= _maxPoint1.position.x)
            {
                transform.position = new Vector3(_maxPoint1.position.x, transform.position.y, transform.position.z);
                _speed = -_speed;
            }
            else if (transform.position.x >= _maxPoint2.position.x)
            {
                transform.position = new Vector3(_maxPoint2.position.x, transform.position.y, transform.position.z);
                _speed = -_speed;
            }
        }
        else if (Time.time - _entryTime >= _existTime)
            gameObject.SetActive(false);

        transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;
    }

    private void VerticalMove()
    {
        if (_needMinMax)
        {
            if (transform.position.y >= _maxPoint1.position.y)
            {
                transform.position = new Vector3(transform.position.x, _maxPoint1.position.y, transform.position.z);
                _speed = -_speed;
            }
            else if (transform.position.y <= _maxPoint2.position.y)
            {
                transform.position = new Vector3(transform.position.x, _maxPoint2.position.y, transform.position.z);
                _speed = -_speed;
            }
        }
        else if (Time.time - _entryTime >= _existTime)
            gameObject.SetActive(false);

        transform.position += new Vector3(0, _speed, 0) * Time.deltaTime;
    }
}
