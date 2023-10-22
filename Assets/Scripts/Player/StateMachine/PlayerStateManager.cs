using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

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
    [SerializeField] private float vX = 5f;
    [SerializeField] private float vY = 10.0f;
    [SerializeField] private AudioSource jumpSound;

    //GET Functions
    public float GetDirX() { return this.dirX; }

    public float GetDirY() { return this.dirY; }

    public bool GetIsOnGround() { return this.IsOnGround; }

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    private Animator anim;

    public Animator GetAnimator() { return this.anim; }

    public AudioSource GetJumpSound() { return this.jumpSound; }

    public float GetvX() { return this.vX; }

    public float GetvY() { return this.vY; }

    //SET Functions
    public void SetIsOnGround(bool para) { this.IsOnGround = para; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        state = idleState;
        //Debug.Log("PSM Called");
        state.EnterState(this);
    }

    public override void ChangeState(BaseState state)
    {
        this.state.ExitState();
        this.state = state;
        //Debug.Log("Called");
        this.state.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            IsOnGround = true;
        }
    }

    
    void Update()
    {
        HandleInput();
        state.UpdateState();
        //Debug.Log("Axis Hori: " + dirX);
    }

    private void FixedUpdate()
    {
        state.FixedUpdate();
    }

    private void HandleInput()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");
    }

    public void FlippingSprite()
    {
        if (dirX < 0)
            sprite.flipX = true;
        else if(dirX > 0)
            sprite.flipX = false;

        //Hàm này dùng để lật sprite theo chiều ngang
    }
}