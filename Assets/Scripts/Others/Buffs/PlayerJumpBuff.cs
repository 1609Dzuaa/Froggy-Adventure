using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpBuff : PlayerBuffs
{
    [SerializeField] private float _jumpMultiplier;

    public float JumpMutiplier { get { return _jumpMultiplier; } }

    public override void Update()
    {
        if (_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _buffDuration)
            {
                _isAllowToUpdate = false;
                //Debug.Log("Timeout!");
            }
            /*else
                Debug.Log("dang buff Jump");*/
        }
    }

}