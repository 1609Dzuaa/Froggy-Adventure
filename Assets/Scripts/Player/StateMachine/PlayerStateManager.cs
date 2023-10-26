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

    private float dirX, dirY;
    private Rigidbody2D rb;
    private bool IsOnGround = false;
    private bool HasDbJump = false; //Cho phép DbJump 1 lần
    private int isLeft = 0; //hướng va chạm với Wall là trái
    private int isRight = 0;
    private int OrangeCount = 0;
    [SerializeField] private float vX = 5f;
    [SerializeField] private float vY = 10.0f;
    [SerializeField] private Text txtScore;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private AudioSource deadSound;

    //GET Functions
    public float GetDirX() { return this.dirX; }

    public float GetDirY() { return this.dirY; }

    public bool GetIsOnGround() { return this.IsOnGround; }

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public AudioSource GetJumpSound() { return this.jumpSound; }

    public float GetvX() { return this.vX; }

    public float GetvY() { return this.vY; }

    public bool GetHasDbJump() { return this.HasDbJump; }

    public int GetLeft() { return this.isLeft; }

    public int GetRight() { return this.isRight; }

    //SET Functions
    public void SetIsOnGround(bool para) { this.IsOnGround = para; }

    public void SetHasDbJump(bool para) { this.HasDbJump = para; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        state = idleState;
        state.EnterState(this);
    }

    public override void ChangeState(BaseState state)
    {
        this.state.ExitState();
        this.state = state;
        this.state.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Platform"))
        {
            IsOnGround = true;
            HasDbJump = false; //Player chạm đất thì mới cho DbJump tiếp
        }
        else if (collision.collider.CompareTag("Trap"))
        {
            HandleDeadState();
        }
        else if (collision.collider.CompareTag("Wall") && !IsOnGround)
        {
            HasDbJump = false;
            Vector2 WallPoint = collision.GetContact(0).point;

            //Quy chiếu: Left = -1, Right = 1; None = 0;
            if(transform.position.x > WallPoint.x) 
            {
                isLeft = 0;
                isRight = 1;
            }
            if (transform.position.x < WallPoint.x)
            {
                isLeft = -1;
                isRight = 0;
            }

            ChangeState(wallSlideState);
            //Xem lại vì có thể run chạm wall => wallSlide
            //Solution: Thêm check trên không :)
            //Vì check va chạm mới cho wallSlide 
            //=> Có thể có TH bị overlap BoxCollider2D
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
        //Debug.Log("Grav Scale: " + rb.gravityScale);
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
}