using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonDashController : ButtonSkillController
{
    private bool _isDashing = false;

    public bool IsDashing { get => _isDashing; }

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(_isActivated);
    }


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
            _isDashing = true;
        else
            _isDashing = false;
    }
}
