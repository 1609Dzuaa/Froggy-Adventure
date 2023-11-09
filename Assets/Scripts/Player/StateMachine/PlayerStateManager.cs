using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerStateManager : BaseStateManager
{
    //PlayerStateManager - Context Class, Use to pass DATA to each State
    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public DoubleJumpState doubleJumpState = new DoubleJumpState();
    public WallSlideState wallSlideState = new WallSlideState();
    public GotHitState gotHitState = new GotHitState();

    private float dirX, dirY;
    private Rigidbody2D rb;
    private bool IsOnGround = false;
    private bool HasDbJump = false; //Cho phép DbJump 1 lần
    private bool IsWallTouch = false;
    private bool isFacingRight = true;
    private bool prevStateIsWallSlide = false;
    private int OrangeCount = 0;

    //Should we put it here ?
    [SerializeField] private Text txtScore;
    private static int HP = 4;

    [Header("Speed")]
    [SerializeField] private float speedX = 5f;
    [SerializeField] private float speedY = 10.0f;
    [SerializeField] private float wallSlideSpeed = 2.0f;
    [SerializeField] private float wallJumpSpeedX;
    [SerializeField] private float wallJumpSpeedY;

    [Header("Force")]
    [SerializeField] private float knockBackForce = 15f;

    [Header("Sound")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private AudioSource gotHitSound;
    [SerializeField] private AudioSource deadSound;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 1.0f;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance;

    //GET Functions
    public float GetDirX() { return this.dirX; }

    public float GetDirY() { return this.dirY; }

    public bool GetIsOnGround() { return this.IsOnGround; }

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public AudioSource GetJumpSound() { return this.jumpSound; }

    public AudioSource GetGotHitSound() { return this.gotHitSound; }

    public float GetSpeedX() { return this.speedX; }

    public float GetSpeedY() { return this.speedY; }

    public float GetWallSlideSpeed() { return this.wallSlideSpeed; }

    public bool GetHasDbJump() { return this.HasDbJump; }

    public bool GetIsWallTouch() { return this.IsWallTouch; }

    public bool GetPrevStateIsWallSlide() { return this.prevStateIsWallSlide; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public float GetWallJumpSpeedX() { return this.wallJumpSpeedX; }

    public float GetWallJumpSpeedY() { return this.wallJumpSpeedY; }

    public float GetKnockBackForce() { return this.knockBackForce; }

    //SET Functions
    //public void SetIsOnGround(bool para) { this.IsOnGround = para; }

    public void SetHasDbJump(bool para) { this.HasDbJump = para; }

    //public void SetPrevStateIsWallSlide(bool para) { this.prevStateIsWallSlide = para; }

    //public void SetIsFacingRight(bool para) { this.isFacingRight = para; }

    //HP Functions
    public void IncreaseHP() { HP++; }

    public void DecreaseHP() { HP--; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        state = idleState;
        state.EnterState(this);
        //Để ý nếu kh có Friction thì nó sẽ bị trôi dù idle

        //Parallax BG: |Origin + (Travel x Parallax)|
        //Trong đó:
        //Origin: Starting Position of Sprites ?
        //Travel: The amount of The Camera has traveled
        //Parallax: Const Value of that Layer
        //(The more it farther, the bigger the Value is)
    }

    public override void ChangeState(BaseState state)
    {
        this.state.ExitState();
        this.state = state;
        //Vì SW là state đặc biệt(phải flip sprite ngược lại sau khi exit state)
        //nên cần đoạn dưới để check
        if (state is WallSlideState)
            prevStateIsWallSlide = true;
        this.state.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Platform"))
        {
            HandleCollideGround();
        }
        else if (collision.collider.CompareTag("Trap") && state is not GotHitState)
        {
            //if (HP > 0)
                ChangeState(gotHitState);
            //else
               // HandleDeadState();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Orange"))
        {
            HandleCollideItem(collision);
        }
        if(collision.CompareTag("Platform"))
        {
            this.transform.SetParent(collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Platform"))
        {
            this.transform.SetParent(null);
        }
    }

    void Update()
    {
        HandleInput();
        state.UpdateState();
        GroundAndWallCheck();
        HandleFlipSprite();
        //Debug.Log("IsWT: " + IsWallTouch);
    }

    private void OnDrawGizmos()
    {
        //Draw Ground Check
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        //Draw Wall Check
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
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
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    public void FlipSpriteAfterWallSlide()
    {
        FlippingSprite();
        prevStateIsWallSlide = false;
        //Hàm này để xử lý việc lật sprite sau khi WS
    }

    private void HandleFlipSprite()
    {
        if (dirX > 0f && !isFacingRight)
        {
            FlippingSprite();
        }
        else if (dirX < 0f && isFacingRight)
        {
            FlippingSprite();
        }
    }

    private void GroundAndWallCheck()
    {
        IsOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallLayer);
        if (isFacingRight)
            IsWallTouch = Physics2D.Raycast(wallCheck.position, Vector2.right, -wallCheckDistance, wallLayer);
        else
            IsWallTouch = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);
    }

    private void Reload()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void ChangeToIdle()
    {
        gotHitState.SetAllowUpdate(true);
    }

    private void HandleCollideItem(Collider2D collision)
    {
        Destroy(collision.gameObject);
        OrangeCount++;
        txtScore.text = "Oranges:" + OrangeCount.ToString();
        collectSound.Play();
    }

    private void HandleDeadState()
    {
        anim.SetTrigger("dead");
        rb.bodyType = RigidbodyType2D.Static;
        deadSound.Play();
    }

    private void HandleCollideGround()
    {
        IsOnGround = true;
        HasDbJump = false; //Player chạm đất thì mới cho DbJump tiếp
    }

}