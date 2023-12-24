using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoManager : MEnemiesManager
{
    //Rhino vẫn flip linh tinh ?@

    private RhinoAttackState _rhinoAttackState = new();
    private RhinoWallHitState _rhinoWallHitState = new();

    [Tooltip("Mở rộng từ phần Time ở class MEnemiesManager")]
    [SerializeField] protected float _restDelay;

    private bool _isHitShield;

    public bool IsHitShield { get { return _isHitShield; } set { _isHitShield = value; } }

    public RhinoWallHitState RhinoWallHitState { get { return this._rhinoWallHitState; } }

    public float RestDelay { get { return this._restDelay; } }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        MEnemiesAttackState = _rhinoAttackState;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        HandleIfCollidedWithShield(collision);
    }

    private void HandleIfCollidedWithShield(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConstants.SHIELD_TAG))
        {
            if (_isFacingRight)
                _rb.velocity = (_knockForce * new Vector2(-1f, 1f));
            else
                _rb.velocity = (_knockForce);
            _isHitShield = true;
            ChangeState(_rhinoWallHitState);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //Event của Wall Hit animation
    private void AllowUpdateWallHit()
    {
        _rhinoWallHitState.AllowUpdate();
    }
}
