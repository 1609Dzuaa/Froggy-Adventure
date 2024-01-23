using System;
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
    public IdleState idleState = new();
    public RunState runState = new();
    public JumpState jumpState = new();
    public FallState fallState = new();
    public DoubleJumpState doubleJumpState = new();
    public WallSlideState wallSlideState = new();
    public GotHitState gotHitState = new();
    public WallJumpState wallJumpState = new();
    public DashState dashState = new();

    private float dirX, dirY;
    private float _jumpStart;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer _spriteRenderer;
    private RaycastHit2D wallHit;
    private bool isOnGround = false;
    private bool _canDbJump = false; //Cho phép DbJump 1 lần
    private bool IsWallTouch = false;
    private bool isFacingRight = true;
    private bool prevStateIsWallSlide = false;
    private bool hasSpawnDust = false;
    private bool _isInteractingWithNPC;
    private bool _hasChange;
    private bool _hasFlip;
    private bool _hasDetectedNPC;
    private bool _hasBeenDisabled;
    private bool _isApplyGotHitEffect;
    private bool _hasStartCoroutine;
    private Vector2 _interactPosition;
    private bool _isVunerable;
    private bool _isHitFromRightSide;

    [Header("Dust")]
    [SerializeField] ParticleSystem dustPS;
    private ParticleSystem.VelocityOverLifetimeModule dustVelocity;
    //Simulation SPACE: Local/World:
    //Chọn Local sẽ làm các hạt di chuyển "link" với local ở đây là vật chứa nó
    //Chọn World sẽ giải phóng các hạt, cho phép chúng di chuyển mà 0 bị "link" với vật chứa nó   

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

    [Header("SO")]
    [SerializeField] private PlayerStats _playerStats;
   
    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer _trailRenderer;

    [Header("Dashable Sign")]
    [SerializeField] private Transform _dashableSignPos;

    //GET Functions

    public Transform DashableSignPosition { get { return _dashableSignPos; } }

    public float JumpStart { get { return _jumpStart; } set { _jumpStart = value; } }

    public float GetDirX() { return dirX; }

    public float GetDirY() { return dirY; }

    public bool GetIsOnGround() { return isOnGround; }

    public Rigidbody2D GetRigidBody2D() { return rb; }

    public Animator GetAnimator() { return anim; }

    public bool GetCanDbJump() { return _canDbJump; }

    public bool GetIsWallTouch() { return IsWallTouch; }

    public bool GetPrevStateIsWallSlide() { return prevStateIsWallSlide; }
    
    public bool GetIsFacingRight() { return isFacingRight; }

    public bool IsInteractingWithNPC { get { return _isInteractingWithNPC; } set { _isInteractingWithNPC = value; } }

    public ParticleSystem GetDustPS() { return dustPS; }

    public TrailRenderer GetTrailRenderer() { return _trailRenderer; }

    public RaycastHit2D WallHit { get { return wallHit; } }

    public bool HasDetectedNPC { get { return _hasDetectedNPC; } }

    public bool IsHitFromRightSide { get => _isHitFromRightSide; set => _isHitFromRightSide = value; }

    public bool IsVunerable { set => _isVunerable = value; }

    //SET Functions
    public void SetCanDbJump(bool para) { this._canDbJump = para; }

    public Vector2 InteractPosition { get { return _interactPosition; } set { _interactPosition = value; } }

    public PlayerStats GetPlayerStats { get { return _playerStats; } set { _playerStats = value; } } 

    public bool IsApplyGotHitEffect { set { _isApplyGotHitEffect = value; } }
    
    private void Awake()
    {
        InitReference();
    }

    private void InitReference()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dustVelocity = GameObject.Find("Dust").GetComponent<ParticleSystem>().velocityOverLifetime;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        RegisterFunction();
        SetupProperties();
    }

    private void RegisterFunction()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.PlayerOnTakeDamage, BeingDamaged);
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.PlayerOnJumpPassive, JumpPassive);
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.PlayerOnInteractWithNPCs, InteractWithNPC);
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.PlayerOnStopInteractWithNPCs, StopInteractWithNPC);
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.PlayerOnBeingPushedBack, PushBack);
    }

    private void SetupProperties()
    {
        _state = idleState;
        _state.EnterState(this);
        rb.gravityScale = _playerStats.GravScale;
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.PlayerOnTakeDamage, BeingDamaged);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.PlayerOnJumpPassive, JumpPassive);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.PlayerOnInteractWithNPCs, InteractWithNPC);
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.PlayerOnStopInteractWithNPCs, StopInteractWithNPC);
    }

    public void ChangeState(PlayerBaseState state)
    {
        //Khi tương tác với NPC chỉ cho change giữa 2 state là Run và Idle
        if (_isInteractingWithNPC && state is not RunState && state is not IdleState)
            return;

        //Nếu state kế vẫn là GotHit mà đang trong thgian miễn dmg thì 0 change
        if (state is GotHitState && Time.time - gotHitState.EntryTime <= _playerStats.InvulnerableTime && _isVunerable)
            return;

        //Thêm đoạn check dưới nếu Upcoming state là GotHit
        //và đang có khiên thì 0 cho change vì có thể có TH
        //chọc xuyên qua collider của shield

        if (state is GotHitState && BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Shield).IsAllowToUpdate)
        {
            //Debug.Log("Tao co khien");
            return;
        }

        _state.ExitState();
        _state = state;
        //Vì SW là state đặc biệt(phải flip sprite ngược lại sau khi exit state)
        //nên cần đoạn dưới để check
        if (state is WallSlideState)
            prevStateIsWallSlide = true;
        _state.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConstants.GROUND_TAG) || collision.collider.CompareTag(GameConstants.PLATFORM_TAG))
            HandleCollideGround();
        else if (collision.collider.CompareTag(GameConstants.TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            ChangeState(gotHitState);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConstants.TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            ChangeState(gotHitState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLATFORM_TAG))
            transform.SetParent(collision.gameObject.transform);
        else if (collision.CompareTag(GameConstants.TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            ChangeState(gotHitState);
        }          
        else if (collision.CompareTag(GameConstants.PORTAL_TAG))
            GameManager.Instance.SwitchToNextScene();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            ChangeState(gotHitState);
        }
        //Tránh TH Overlap
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLATFORM_TAG))
            transform.SetParent(null);
    }

    private void BeingDamaged(object obj)
    {
        _isHitFromRightSide = (bool)obj;
        //Debug.Log("bi hit tu right: " + (bool)obj);
        ChangeState(gotHitState);
        //Đky event = func này, không phải gây tight-coupling ở class enemies
        //Enemies đ' cần quan tâm về rb của player, nó chỉ việc phát thông báo (Invoke)
        //Thằng nào có liên quan đến thì đăng ký và xử lý
    }

    private void JumpPassive(object obj)
    {
        _canDbJump = true;
        ChangeState(jumpState);
    }

    private void InteractWithNPC(object obj)
    {
        _isInteractingWithNPC = true;
    }

    private void StopInteractWithNPC(object obj)
    {
        _isInteractingWithNPC = false;
    }

    void Update()
    {
        if (_hasBeenDisabled)
            return;

        //=========Handle things related to NPC==========//
        NPCCheck();
        DrawRayDetectNPC();

        if (_isInteractingWithNPC)
        {
            HandleInteractWithNPC();
            return;
        }
        else
        {
            _hasChange = false;
            _hasFlip = false;
        }
        //=========Handle things related to NPC==========//

        LockIfOutMinBound();
        UpdateLayer();
        HandleInput();
        _state.Update();
        GroundAndWallCheck();
        HandleFlipSprite();
        HandleAlphaValueGotHit();
        HandleDustVelocity();
        SpawnDust();
        //Debug.Log("v: " + rb.velocity);
    }

    private void UpdateLayer()
    {
        //Cơ chế Invunerable của HK:
        //Switch layer cho player trong khoảng thgian miễn dmg
        //Cho phép enemies đâm xuyên qua player và ngược lại
        //Đỡ việc 2 box va nhau, có thể gây khó chịu cho Player.
        //Phải có 1 biến bool để chặn cửa cùng Time, 0 thì đầu game ĐK thoả mãn luôn ^.^
        if (Time.time - gotHitState.EntryTime <= _playerStats.InvulnerableTime && _isVunerable)
            gameObject.layer = LayerMask.NameToLayer(GameConstants.IGNORE_ENEMIES_LAYER);
        else if (_state is not DashState)
        {
            _isVunerable = false;
            gameObject.layer = LayerMask.NameToLayer(GameConstants.PLAYER_LAYER);
        }
    }

    private void HandleInteractWithNPC()
    {
        //Tương tác với NPC thì chỉ xử lý 2 state là Idle và Run
        //Đơn giản là lật sprite (nếu có) về hướng InteractPosition
        HandleFlipTowardsInteractPos();
        AllowMoveTowardsInteractPos();
        _state.Update();
    }

    private void HandleFlipTowardsInteractPos()
    {
        if (!_hasFlip)
        {
            _hasFlip = true;

            if (isFacingRight && transform.position.x > InteractPosition.x + GameConstants.CAN_START_CONVERSATION_RANGE)
            {
                FlippingSprite();
                //Debug.Log("Flip to Left");
            }
            else if (!isFacingRight && transform.position.x < InteractPosition.x - GameConstants.CAN_START_CONVERSATION_RANGE)
            {
                FlippingSprite();
                //Debug.Log("Flip to Right");
            }
        }
    }

    private void AllowMoveTowardsInteractPos()
    {
        if (!_hasChange)
        {
            _hasChange = true;
            Invoke(nameof(ChangeToRun), GameConstants.DELAYPLAYERRUNSTATE);
        }
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
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
        dirX = Input.GetAxisRaw(GameConstants.HORIZONTAL_AXIS);
        dirY = Input.GetAxisRaw(GameConstants.VERTICAL_AXIS);
    }

    public void FlippingSprite()
    {
        //ĐK check if:
        //Vì khi WallJump sau khi WS,
        //ta muốn sprite giữ nguyên ở hướng WS 1 khoảng DisableTime
        //Nên chỉ cho lật sprite ở 2 states đó khi hết DisableTime(IsEndDisable)
        //Tránh việc Player khi vừa WS và bấm S (WJ) nhưng Input directionX
        //hướng về cái Wall vừa đu dẫn đến Player quay mặt luôn vào cái Wall

        if(_state is WallJumpState)
        {
            if(wallJumpState.IsEndDisable)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0, 180, 0);
                //Debug.Log("hereWJ");
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
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        prevStateIsWallSlide = false;
        //Debug.Log("Here");
        //Hàm này để xử lý việc lật sprite sau khi WS
    }

    private void HandleFlipSprite()
    {
        //Sao 0 nghĩ đơn giản hơn là đ' cho flip khi dash 
        if (_state is DashState) 
            return;
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

    private void ChangeToIdle()
    {
        ChangeState(idleState);
    }

    private void ChangeToRun()
    {
        //Dùng để Invoke Delay sang RunState khi tương tác với NPC
        //Tránh TH như mushroom, flip quá nhanh dẫn đến thoả mãn đk quá nhanh
        ChangeState(runState);
    }

    private void AllowUpdateDash()
    {
        dashState.AllowUpdate = true;
        //Vứt trong Dash Animation để Delay việc Update của Dash 
    }

    public void HandleDeadState()
    {
        anim.SetTrigger(GameConstants.DEAD_ANIMATION);
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.layer = LayerMask.NameToLayer("Enemies");
        SoundsManager.Instance.GetTypeOfSound(GameConstants.PLAYER_DEAD_SOUND).Play();
    }

    private void HandleCollideGround()
    {
        isOnGround = true;
        _canDbJump = true; //Player chạm đất thì mới cho DbJump tiếp
    }

    public IEnumerator ReloadScence()
    {
        yield return new WaitForSeconds(1f);

        GameManager.Instance.ReloadScene();
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

    public void Disable()
    {
        ChangeState(idleState);
        _hasBeenDisabled = true;
    }

    public void Enable()
    {
        _hasBeenDisabled = false;
        Debug.Log("Enable");
    }

    private IEnumerator Twinkling()
    {
        //Lock - Đảm bảo chỉ gọi coroutine sau khi đi đc 1 vòng Alpha Value:
        //từ 1 -> AlphaVal -> 1
        _hasStartCoroutine = true;
        _spriteRenderer.color = new Color(1f, 1f, 1f, _playerStats.AlphaValueGotHit);
        //Debug.Log("tang lan: " + _count);

        yield return new WaitForSeconds(_playerStats.TimeEachApplyAlpha);

        //Thêm check đây nữa 
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
        {
            _hasStartCoroutine = false;
            yield return null;
        }
        else
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(_playerStats.TimeEachApplyAlpha);

        _hasStartCoroutine = false;
    }

    private void HandleAlphaValueGotHit()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return;

        if (Time.time - gotHitState.EntryTime <= _playerStats.InvulnerableTime && !_hasStartCoroutine && _isApplyGotHitEffect)
            StartCoroutine(Twinkling());
        else if (Time.time - gotHitState.EntryTime > _playerStats.InvulnerableTime)
        {
            //Hết thgian miễn dmg r thì trả màu về như cũ cho nó
            //NẾU trên ng 0 có buff vô hình, còn có thì return và set lại bool
            if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            {
                _isApplyGotHitEffect = false;
                return;
            }

            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _isApplyGotHitEffect = false;
        }
    }

    private void SpawnDashableEffect()
    {
        GameObject dEff = EffectPool.Instance.GetObjectInPool(GameConstants.DASHABLE_EFFECT);
        dEff.SetActive(true);
        //Event của Dash animation
        //Dùng để ra dấu hiệu chỉ đc dash khi hết effect
    }

    private void LockIfOutMinBound()
    {
        if (transform.position.x < GameConstants.GAME_MIN_BOUNDARY)
            transform.position = new Vector3(GameConstants.GAME_MIN_BOUNDARY, transform.position.y, transform.position.z);
    }

    private void PushBack(object obj)
    {
        Vector2 force = (Vector2)obj;
        rb.AddForce(isFacingRight ? force * new Vector2(-1f, 1f) : force);
    }

}