using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedBuff : PlayerBuffs
{
    //Buff này chắc dùng PS/TR để tạo effect
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private Transform _speedBuffIcon; //sign báo hiệu vẫn còn thgian buff
    [SerializeField] private Transform _speedBuffIconPos;

    public float SpeedMultiplier { get { return _speedMultiplier; } }

    public override void Start()
    {
        _speedBuffIcon.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _buffDuration)
            {
                _isAllowToUpdate = false;
                _speedBuffIcon.gameObject.SetActive(false);
                Debug.Log("Timeout!");
            }
            /*else
                Debug.Log("dang buff Speed");*/
            _speedBuffIcon.transform.position = _speedBuffIconPos.position;
        }
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        _speedBuffIcon.gameObject.SetActive(true);
        _speedBuffIcon.transform.position = _speedBuffIconPos.position;
    }

}

