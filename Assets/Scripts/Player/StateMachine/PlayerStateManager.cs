using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStateManager : BaseStateManager
{
    //Context Class, Use to pass DATA to each State
    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    //2 States dưới đụng sau
    //public WallJumpState wallJumpState = new WallJumpState();
    //public DashState dashState = new DashState();

    private float dirX, dirY;
    private Rigidbody2D rb;
    private bool IsOnGround = false;

    public float GetDirX() { return this.dirX; }

    public float GetDirY() { return this.dirY; }

    public bool GetIsOnGround() { return this.IsOnGround; }

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        state = idleState;
        state.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            IsOnGround = true;
        }
    }

    /*
    void Update()
    {
        state.UpdateState(this);
    }

    private void FixedUpdate()
    {
        state.FixedUpdate(this);
    }*/

}