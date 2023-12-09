using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoManager : MEnemiesManager
{
    private RhinoAttackState _rhinoAttackState = new();
    private RhinoWallHitState _rhinoWallHitState = new();

    [Tooltip("Mở rộng từ phần Time ở class MEnemiesManager")]
    [SerializeField] protected float _restDelay;

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
