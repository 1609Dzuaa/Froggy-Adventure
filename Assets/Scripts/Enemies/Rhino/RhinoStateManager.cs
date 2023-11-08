using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RhinoStateManager : BaseStateManager
{
    public RhinoIdleState RhinoIdleState = new RhinoIdleState();
    public RhinoRunState RhinoRunState = new RhinoRunState();
    public RhinoWallHitState RhinoWallHitState = new RhinoWallHitState();

    [Header("Speed")]
    [SerializeField] private float runSpeed = 5.0f;

    //Private Field
    private Rigidbody2D rb;
    private bool isFacingRight = false;

    //Public Func
    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public float GetRunSpeed() { return this.runSpeed; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        state = RhinoIdleState;
        state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        state.UpdateState();
    }

    private void FixedUpdate()
    {
        state.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            ChangeState(RhinoWallHitState);
    }
}
