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
    private int isLeft = 0; //hướng va chạm với Wall là trái
    private int isRight = 0;
    private bool isFacingRight = true;
    private bool prevStateIsWallSlide = false;
    private int OrangeCount = 0;

    //Should we put it here ?
    [SerializeField] private Text txtScore;
    private static int HP = 4;

    [Header("Velocity")]
    [SerializeField] private float vX = 5f;
    [SerializeField] private float vY = 10.0f;
    [SerializeField] private float wallSlideSpeed = 2.0f;

    [Header("Sound")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource collectSound;
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

    public float GetvX() { return this.vX; }

    public float GetvY() { return this.vY; }

    public float GetWallSlideSpeed() { return this.wallSlideSpeed; }

    public bool GetHasDbJump() { return this.HasDbJump; }

    public bool GetIsWallTouch() { return this.IsWallTouch; }

    public int GetLeft() { return this.isLeft; }

    public int GetRight() { return this.isRight; }

    public bool GetPrevStateIsWallSlide() { return this.prevStateIsWallSlide; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    //SET Functions
    public void SetIsOnGround(bool para) { this.IsOnGround = para; }

    public void SetHasDbJump(bool para) { this.HasDbJump = para; }

    public void SetPrevStateIsWallSlide(bool para) { this.prevStateIsWallSlide = para; }

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
    }

    public override void ChangeState(BaseState state)
    {
        this.state.ExitState();
        this.state = state;
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
        else if (collision.collider.CompareTag("Trap"))
        {
            if (HP > 0)
                ChangeState(gotHitState);
            else
                HandleDeadState();
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
        IsOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallLayer);
        IsWallTouch = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);

        if (rb.velocity.x > 0.1f && !isFacingRight)
        {
            FlippingSprite();
        }
        else if (rb.velocity.x < -0.1f && isFacingRight)
        {
            FlippingSprite();
        }

        //Debug.Log("FR: " + isFacingRight);
        /*if (IsWallTouch)
        {
            Debug.Log("Normal: " + ray.normal.x);
        }*/

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

        //Cách cũ thì chỉ đơn giản là lật sprite thôi
        //Bỏ cách cũ
        /*if (dirX < 0)
            sprite.flipX = true;
        else if (dirX > 0)
            sprite.flipX = false;*/

        //Hàm này dùng để lật sprite theo chiều ngang
    }

    private void Reload()
    {
        SceneManager.LoadScene("Level 1");
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

    private void HandleCollideWall(Collision2D collision)
    {
        //Tạm cất hàm này, tìm cách wall slide hay hơn

        /*HasDbJump = false;
        Vector2 WallPoint = collision.GetContact(0).point;

        //Quy chiếu: Left = -1, Right = 1; None = 0;
        if (transform.position.x > WallPoint.x)
        {
            isLeft = 0;
            isRight = 1;
        }
        if (transform.position.x < WallPoint.x)
        {
            isLeft = -1;
            isRight = 0;
        }

        ChangeState(wallSlideState);*/
    }
}