using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonDashController : ButtonCooldownController
{
    private bool _isDashing = false;

    public bool IsDashing { get => _isDashing; }

    public void Dash(InputAction.CallbackContext context)
    {
        _isDashing = context.performed;
    }
}
