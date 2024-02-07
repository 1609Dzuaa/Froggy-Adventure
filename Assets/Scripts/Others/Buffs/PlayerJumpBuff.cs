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
                SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.NormalBuffDownSfx, 1.0f);
                //Debug.Log("Timeout!");
            }
            /*else
                Debug.Log("dang buff Jump");*/
        }
    }

    public override void ApplyBuff()
    {
        base.ApplyBuff();
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.NormalBuffUpSfx, 1.0f);
    }

}