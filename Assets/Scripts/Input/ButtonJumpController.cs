using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static GameConstants;
using static GameEnums;

public class ButtonJumpController : ButtonSkillController
{
    [SerializeField] PlayerStateManager _player;

    private bool _isHolding = false;
    private bool _dbJump = true;
    int _count = 0;

    public bool IsHolding { get => _isHolding; }

    public bool DbJump { get => _dbJump; }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!_player)
            Debug.Log("quen ref player o script " + this);

        //chạm đất thì reset count
        if (_player.GetIsOnGround())
            _count = 0;

        //Jump
        if (_count == 0)
        {
            if (context.performed)
            {
                //Debug.Log("value: " + context.ReadValue<float>());
                _isHolding = true;
                _dbJump = false;
                _count = 1;
            }
            else if (context.canceled)
            {
                _isHolding = false;
                _dbJump = false;
            }
        }
        else
        {
            if (context.performed)
            {
                _isHolding = false;
                _dbJump = true;
                _count = 0;
            }
            else if (context.canceled)
            {
                _isHolding = false;
                _dbJump = false;
            }
        }
        //Debug.Log("Phase: " + context.phase);
        //Debug.Log("count, holding, frame: " + _count + " " + _isHolding + " " + Time.frameCount);
    }
}
