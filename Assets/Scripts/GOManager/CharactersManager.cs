using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : GameObjectManager
{
    protected bool _isFacingRight = false;
    protected Rigidbody2D _rb;

    public bool GetIsFacingRight() { return _isFacingRight; }

    public Rigidbody2D GetRigidbody2D() { return _rb; }

    protected CharacterBaseState _state;

    protected override void Awake()
    {
        base.Awake(); //Lấy anim từ GObjManager
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    protected virtual void Update()
    {
        if (_state != null)
            _state.Update();
    }

    public void ChangeState(CharacterBaseState state)
    {
        _state.ExitState();
        _state = state;
        _state.EnterState(this);
    }

    public void FlippingSprite()
    {
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
        //Debug.Log("vao day");
    }
}
