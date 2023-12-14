using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerStateManager : MonoBehaviour
{
    //PlayerStateManager - Context Class, Use to pass DATA to each State
    private PlayerBaseState _state;
    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public DoubleJumpState doubleJumpState = new DoubleJumpState();
    public WallSlideState wallSlideState = new WallSlideState();
    public GotHitState gotHitState = new GotHitState();
    public WallJumpState wallJumpState = new();

    private float dirX, dirY;
    private Rigidbody2D rb;
    private Animator anim;
    private RaycastHit2D wallHit;
    private bool isOnGround = false;
    private bool hasDbJump = false; //Cho phép DbJump 1 lần
    private bool IsWallTouch = false;
    private bool isFacingRight = true;
    private bool prevStateIsWallSlide = false;
    private bool hasSpawnDust = false;
    private bool _isInteractingWithNPC;
    private bool _hasChange;
    private bool _hasFlip;
    private bool _hasDetectedNPC;
    private Vector2 _InteractPosition;
    private int OrangeCount = 0;

    //Should we put it here ?
    [SerializeField] private Text txtScore;
    private static int HP = 4;

    [Header("Dust")]
    [SerializeField] ParticleSystem dustPS;
    private ParticleSystem.VelocityOverLifetimeModule dustVelocity;
    ParticleSystem.MinMaxCurve rate;
    //Simulation SPACE: Local/World:
    //Chọn Local sẽ làm các hạt di chuyển "link" với local ở đây là vật chứa nó
    //Chọn World sẽ giải phóng các hạt, cho phép chúng di chuyển mà 0 bị "link" với vật chứa nó

    [Header("Speed")]
    [SerializeField] private float speedX = 5f;
    [SerializeField] private float speedY = 10.0f;
    [SerializeField] private float wallSlideSpeed = 2.0f;
    [SerializeField] private float wallJumpSpeedX;
    [SerializeField] private float wallJumpSpeedY;

    [Header("Force")]
    [SerializeField] private float knockBackForce = 15f;
    [SerializeField] private Vector2 jumpOnEnemiesForce;

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

    [Header("NPC Check")]
    [SerializeField] private Transform _npcCheck;
    [SerializeField] private LayerMask _npcLayer;
    [SerializeField] private float _npcCheckDistance;

    [Header("Time")]
    //Là khoảng thgian mà mình disable directionX ở state WallJump
    //Mục đích cho nó nhảy hướng ra phía đối diện cái wall vừa đu
    //Mà 0 bị ảnh hưởng bởi input directionX
    [SerializeField] private float _disableTime;

    public float DisableTime { get { return this._disableTime; } }

    //GET Functions
    public float GetDirX() { return this.dirX; }

    public float GetDirY() { return this.dirY; }

    public bool GetIsOnGround() { return this.isOnGround; }

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public Animator GetAnimator() { return this.anim; }

    public AudioSource GetJumpSound() { return this.jumpSound; }

    public AudioSource GetGotHitSound() { return this.gotHitSound; }

    public float GetSpeedX() { return this.speedX; }

    public float GetSpeedY() { return this.speedY; }

    public float GetWallSlideSpeed() { return this.wallSlideSpeed; }

    public bool GetHasDbJump() { return this.hasDbJump; }

    public bool GetIsWallTouch() { return this.IsWallTouch; }

    public bool GetPrevStateIsWallSlide() { return this.prevStateIsWallSlide; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public bool IsInteractingWithNPC { get { return _isInteractingWithNPC; } set { _isInteractingWithNPC = value; } }

    public float GetWallJumpSpeedX() { return this.wallJumpSpeedX; }

    public float GetWallJumpSpeedY() { return this.wallJumpSpeedY; }

    public float GetKnockBackForce() { return this.knockBackForce; }

    public Vector2 GetJumpOnEnemiesForce() { return this.jumpOnEnemiesForce; }

    public ParticleSystem GetDustPS() { return this.dustPS; }

    public int GetOrangeCount() { return this.OrangeCount; }

    public AudioSource GetCollectSound() { return this.collectSound; }

    public Text GetScoreText() { return this.txtScore; }

    public RaycastHit2D WallHit { get { return this.wallHit; } }

    public bool HasDetectedNPC { get { return _hasDetectedNPC; } }

    //SET Functions
    public void SetHasDbJump(bool para) { this.hasDbJump = para; }

    public Vector2 InteractPosition { get { return _InteractPosition; } set { _InteractPosition = value; } }

    public void IncreaseOrangeCount() { this.OrangeCount++; }

    //HP Functions
    public void IncreaseHP() { HP++; }

    public void DecreaseHP() { HP--; }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dustVelocity = GameObject.Find("Dust").GetComponent<ParticleSystem>().velocityOverLifetime;
        _state = idleState;
        _state.EnterState(this);

        //Parallax BG: |Origin + (Travel x Parallax)|
        //Trong đó:
        //Origin: Starting Position of Sprites ?
        //Travel: The amount of The Camera has traveled
        //Parallax: Const Value of that Layer
        //(The more it farther, the bigger the Value is)
    }

    public void ChangeState(PlayerBaseState state)
    {
        //Khi tương tác với NPC chỉ cho change giữa 2 state là Run và Idle
        if (_isInteractingWithNPC && state is not RunState && state is not IdleState)
            return;

        this._state.ExitState();
        this._state = state;
        //Vì SW là state đặc biệt(phải flip sprite ngược lại sau khi exit state)
        //nên cần đoạn dưới để check
        if (state is WallSlideState)
            prevStateIsWallSlide = true;
        this._state.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Platform"))
        {
            HandleCollideGround();
        }
        else if (collision.collider.CompareTag("Trap") && _state is not GotHitState
            || collision.collider.CompareTag("Enemy") && _state is not GotHitState
            || collision.collider.CompareTag("Bullet") && _state is not GotHitState)
        {
            //if (HP > 0)
                ChangeState(gotHitState);
            //else
               // HandleDeadState();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        NPCCheck();
        DrawRayDetectNPC();

        if (_isInteractingWithNPC)
        {
            UpdateInteractWithNPC();
            return;
        }
        else
        {
            _hasChange = false;
            _hasFlip = false;
        }

        HandleInput();
        _state.Update();
        GroundAndWallCheck();
        HandleFlipSprite();
        HandleDustVelocity();
        SpawnDust();
        //Debug.Log("dirX: " + dirX);
    }

    private void UpdateInteractWithNPC()
    {
        //Tương tác với NPC thì chỉ xử lý 2 state là Idle và Run
        if (IsInteractingWithNPC && !_hasFlip)
        {
            _hasFlip = true;
            if (isFacingRight)
            {
                if (transform.position.x > InteractPosition.x + GameConstants.STARTCONVERSATIONRANGE)
                {
                    FlippingSprite();
                    Debug.Log("Flip to Left");
                }
            }
            else
            {
                if (transform.position.x < InteractPosition.x - GameConstants.STARTCONVERSATIONRANGE)
                {
                    FlippingSprite();
                    Debug.Log("Flip to Right");
                }
            }
        }
        if(!_hasChange)
        {
            _hasChange = true;
            //Debug.Log("Change");
            Invoke("ChangeToRun", GameConstants.DELAYPLAYERRUNSTATE);
        }    
        _state.Update();
    }

    private void ChangeToRun()
    {
        //Dùng để Invoke Delay sang RunState khi tương tác với NPC
        //Tránh TH như mushroom, flip quá nhanh dẫn đến thoả mãn đk quá nhanh
        ChangeState(runState);
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
        //Unity Docs:
        //If a hit starts occuring inside a collider, the collision normal is
        //the OPPOSITE direction of the line/ray query.
        //Giữ A va trái thì normalX = 1 (với isFr = false)
    }

    private void OnDrawGizmos()
    {
        //Draw Ground Check
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        //Draw Wall Check
        if (isFacingRight)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        else
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void HandleInput()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");
    }

    public void FlippingSprite()
    {
        //ĐK check if:
        //Vì khi WallJump, ta muốn sprite giữ nguyên ở hướng WallSlide 1 khoảng DisableTime
        //Nên chỉ cho lật sprite khi hết DisableTime(IsEndDisable)
        if(_state is WallJumpState)
        {
            if(wallJumpState.IsEndDisable)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180, 0);
            }
        }
        else
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    public void FlipSpriteAfterWallSlide()
    {
        //FlippingSprite();
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        prevStateIsWallSlide = false;
        //Debug.Log("Here");
        //Hàm này để xử lý việc lật sprite sau khi WS
    }

    private void HandleFlipSprite()
    {
        //Tại sao thêm ĐK !IsWallTouch
        //Vì sprites Wall Slide bị ngược + lười chỉnh :v
        //Và vì sau khi WallSlide xong thì sprite phải được
        //lật ngược cho phù hợp 
        if(!IsWallTouch)
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
    }

    private void GroundAndWallCheck()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallLayer);
        if (isFacingRight)
        {
            IsWallTouch = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);
            wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);
        }
        else
        {
            IsWallTouch = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wallLayer);
            wallHit = Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wallLayer);
        }
    }

    private void NPCCheck()
    {
        if(isFacingRight)
            _hasDetectedNPC = Physics2D.Raycast(_npcCheck.position, Vector2.right, _npcCheckDistance, _npcLayer);
        else
            _hasDetectedNPC = Physics2D.Raycast(_npcCheck.position, Vector2.left, _npcCheckDistance, _npcLayer);
    }

    private void DrawRayDetectNPC()
    {
        if(!_hasDetectedNPC)
        {
            if (isFacingRight)
                Debug.DrawRay(_npcCheck.position, Vector2.right * _npcCheckDistance, Color.green);
            else
                Debug.DrawRay(_npcCheck.position, Vector2.left * _npcCheckDistance, Color.green);
        }
        else
        {
            if (isFacingRight)
                Debug.DrawRay(_npcCheck.position, Vector2.right * _npcCheckDistance, Color.red);
            else
                Debug.DrawRay(_npcCheck.position, Vector2.left * _npcCheckDistance, Color.red);
        }
    }

    private void Reload()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void ChangeToIdle()
    {
        gotHitState.SetAllowUpdate(true);
    }

    private void HandleDeadState()
    {
        anim.SetTrigger("dead");
        rb.bodyType = RigidbodyType2D.Static;
        deadSound.Play();
    }

    private void HandleCollideGround()
    {
        isOnGround = true;
        hasDbJump = false; //Player chạm đất thì mới cho DbJump tiếp
    }

    private void SpawnDust()
    {
        if (isOnGround && !hasSpawnDust)
        {
            hasSpawnDust = true;
            dustPS.Play();
        }
        else if (!isOnGround)
            hasSpawnDust = false;
        //Chạm đất thì Spawn Dust 1 lần
    }

    private void HandleDustVelocity()
    {
        if (isFacingRight)
            dustVelocity.x = -0.3f;
        else
            dustVelocity.x = 0.3f;

        //Nhược điểm lớn là nếu thay đổi velo ngoài Inspector phải vào đây sửa @@ 
    }
}