using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameEnums;
using static GameConstants;
using Unity.Profiling;

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
    private float _startCoyote;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _capCollider2D;
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
    private bool _isOnPlatform;
    private bool _hasDead; //Tránh HandleDeadState bị gọi nhiều lần
    private bool _hasStart;
    private bool _canJump;
    private bool _forceApply;
    private bool _hasWinGame;
    private bool _isMagnetized;
    private bool _isCursed;

    [HideInInspector] public bool UnlockedDbJump;
    [HideInInspector] public bool UnlockedWallSlide;
    private bool _unlockedDash;

    HashSet<Collider2D> _hashCoinsMagnetized = new();
    Collider2D[] _arrCoinsMagnetized;
    private static ProfilerMarker performanceMarker = new ProfilerMarker("ImprovedCode");

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
    [SerializeField] private LayerMask _coinLayer;

    [Header("NPC Check")]
    [SerializeField] private Transform _npcCheck;
    [SerializeField] private LayerMask _npcLayer;
    [SerializeField] private float _npcCheckDistance;

    [Header("SO")]
    [SerializeField] private PlayerStats _playerStats;
   
    [Header("Trail Renderer")]
    //[SerializeField] private TrailRenderer _trailRenderer;

    [Header("Dashable Sign")]
    [SerializeField] private Transform _dashableSignPos;

    [SerializeField] private Joystick _joystick;

    [Header("Button Skill References")]
    [SerializeField] private ButtonJumpController _btnJumpControl;
    [SerializeField] private ButtonDashController _btnDashControl;

    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public float JumpSpeed;

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

    //public TrailRenderer GetTrailRenderer() { return _trailRenderer; }

    public RaycastHit2D WallHit { get { return wallHit; } }

    public bool HasDetectedNPC { get { return _hasDetectedNPC; } }

    public bool IsHitFromRightSide { get => _isHitFromRightSide; set => _isHitFromRightSide = value; }

    public bool IsVunerable { set => _isVunerable = value; }

    public bool CanJump { get => _canJump; }

    public bool ForceApply { get => _forceApply; }

    public ButtonJumpController BtnJumpControl { get => _btnJumpControl; set => _btnJumpControl = value; }

    public ButtonDashController BtnDashControl { get => _btnDashControl; set => _btnDashControl = value; }

    //SET Functions
    public void SetCanDbJump(bool para) { _canDbJump = para; }

    public Vector2 InteractPosition { get { return _interactPosition; } set { _interactPosition = value; } }

    public PlayerStats GetPlayerStats { get { return _playerStats; } set { _playerStats = value; } } 

    public bool IsApplyGotHitEffect { set { _isApplyGotHitEffect = value; } }
    
    private void Awake()
    {
        GetReferenceComponents();
    }

    private void GetReferenceComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void UpdatePosition()
    {
        if (PlayerPrefs.HasKey(ESpecialStates.PlayerPositionUpdatedX.ToString()))
        {
            float x = PlayerPrefs.GetFloat(ESpecialStates.PlayerPositionUpdatedX.ToString());
            float y = PlayerPrefs.GetFloat(ESpecialStates.PlayerPositionUpdatedY.ToString());
            float z = PlayerPrefs.GetFloat(ESpecialStates.PlayerPositionUpdatedZ.ToString());
            transform.position = new Vector3(x, y, z);
        }
        //Cập nhật vị trí Player dựa trên Checkpoint
    }

    private void HandlePlayerSkills()
    {
        var list = ToggleAbilityItemHelper.GetListActivatedSkills(false);
        UnlockedDbJump = list.Find(x => x.SkillName == ESkills.DoubleJump) != null;
        UnlockedWallSlide = list.Find(x => x.SkillName == ESkills.WallSlide) != null;
        _unlockedDash = list.Find(x => x.SkillName == ESkills.Dash) != null;
    }

    // Start is called before the first frame update
    private void Start()
    {
        RegisterFunction();
        SetupProperties();
        UpdatePosition();
        HandlePlayerSkills();
    }

    private void RegisterFunction()
    {
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnTakeDamage, BeingDamaged);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnJumpPassive, JumpPassive);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnInteractWithNPCs, InteractWithNPC);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnStopInteractWithNPCs, StopInteractWithNPC);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnBeingPushedBack, PushBack);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.PlayerOnUpdateRespawnPosition, UpdateRespawnPosition);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnUnlockSkill, UnlockSkill);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnLevelCompleted, HandleWinGame);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnValidatePlayerBuffs, ValidateBuffs);
    }

    private void SetupProperties()
    {
        _state = idleState;
        _state.EnterState(this);
        rb.gravityScale = _playerStats.GravScale;
    }

    private void OnDestroy()
    {
        UnsubcribeAllEvents();
    }

    private void UnsubcribeAllEvents()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnTakeDamage, BeingDamaged);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnJumpPassive, JumpPassive);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnInteractWithNPCs, InteractWithNPC);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnStopInteractWithNPCs, StopInteractWithNPC);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnBeingPushedBack, PushBack);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.PlayerOnUpdateRespawnPosition, UpdateRespawnPosition);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnUnlockSkill, UnlockSkill);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnLevelCompleted, HandleWinGame);
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnValidatePlayerBuffs, ValidateBuffs);
    }

    public void ChangeState(PlayerBaseState state)
    {
        //Khi tương tác với NPC chỉ cho change giữa 2 state là Run và Idle
        if (_isInteractingWithNPC && state is not RunState && state is not IdleState)
            return;

        //Nếu state kế vẫn là GotHit mà đang trong thgian miễn dmg thì 0 change
        if (state is GotHitState && Time.time - gotHitState.EntryTime <= _playerStats.InvulnerableTime && _isVunerable)
            return;

        if (_hasWinGame && state is not FallState && state is not IdleState)
            return;

        //Thêm đoạn check dưới nếu Upcoming state là GotHit
        //và đang có khiên thì 0 cho change vì có thể có TH
        //chọc xuyên qua collider của shield

        if (state is GotHitState && BuffsManager.Instance.GetBuff(EBuffs.Shield).IsActivating)
        {
            //day
            //Debug.Log("Tao co khien");
            return;
        }

        if (state is DoubleJumpState && !UnlockedDbJump)
            return;
        else if (state is WallSlideState && !UnlockedWallSlide)
            return;
        else if (state is DashState && !_unlockedDash)
            return;

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
        if (collision.collider.CompareTag(GROUND_TAG) || collision.collider.CompareTag(PLATFORM_TAG))
            HandleCollideGround();
        else if (collision.collider.CompareTag(TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            if (_isCursed)
                HandleDeadState();
            else
                ChangeState(gotHitState);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            if (_isCursed)
                HandleDeadState();
            else
                ChangeState(gotHitState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLATFORM_TAG))
        {
            transform.SetParent(collision.gameObject.transform);
            _isOnPlatform = true;
        }
        else if (collision.CompareTag(TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            if (_isCursed)
                HandleDeadState();
            else
                ChangeState(gotHitState);
        }
        else if (collision.CompareTag(DEAD_ZONE_TAG))
            HandleDeadState();
        else if (collision.CompareTag(PORTAL_TAG))
        {
            SoundsManager.Instance.PlaySfx(ESoundName.GreenPortalSfx, 1.0f);
            anim.SetTrigger(DEAD_ANIMATION);
            rb.bodyType = RigidbodyType2D.Static;
            EventsManager.Instance.NotifyObservers(EEvents.OnLevelCompleted, ELevelResult.Completed);
            GameManager.Instance.DeleteInconsistentPrefsKey(); //xoá hết data trong đây khi win level
            //GameManager.Instance.SwitchToScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TRAP_TAG) && _state is not GotHitState)
        {
            gotHitState.IsHitByTrap = true;
            if (_isCursed)
                HandleDeadState();
            else
                ChangeState(gotHitState);
        }
        //Tránh TH Overlap
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PLATFORM_TAG))
        {
            transform.SetParent(null);
            _isOnPlatform = false;
        }
    }

    private void BeingDamaged(object obj)
    {
        if (_isCursed)
            HandleDeadState();
        else
        {
            _isHitFromRightSide = (bool)obj;
            //Debug.Log("bi hit tu right: " + (bool)obj);
            ChangeState(gotHitState);
        }
    }

    private void JumpPassive(object obj)
    {
        //Jump lên đầu quái, hoặc vật nảy thì jump tiếp (passive)
        _canDbJump = true;
        if (obj != null)
            jumpState.JumpForceApplied = (float)obj;
        ChangeState(jumpState);
    }

    private void InteractWithNPC(object obj)
    {
        _isInteractingWithNPC = true;
        _interactPosition = (Vector2)obj;
        //Debug.Log("inter: " + _interactPosition);
    }

    private void StopInteractWithNPC(object obj)
    {
        _isInteractingWithNPC = false;
    }

    void Update()
    {
        if (_hasBeenDisabled)
            return;

        //Chết là hết
        if (PlayerHealthManager.Instance.CurrentHP == 0)
        {
            HandleDeadState();
            return;
        }

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

        UpdateLayer();
        HandleInput();
        HandleCoyoteTime();
        _state.Update();
        HandleFlipSprite();
        HandleAlphaValueGotHit();
        //HandleDustVelocity();
        //SpawnDust();
        //Debug.Log("OG, CanJ: " + isOnGround + ", " + _canJump);
    }

    /// <summary>
    ///Hàm dưới để thực hiện cơ chế Invunerable của HK:
    ///Switch layer cho player trong khoảng thgian miễn dmg
    ///Cho phép enemies đâm xuyên qua player và ngược lại
    ///Đỡ việc 2 box va nhau, có thể gây khó chịu cho Player.
    ///Phải có 1 biến bool để chặn cửa cùng Time, 0 thì đầu game ĐK thoả mãn luôn ^.^
    /// </summary>

    private void UpdateLayer()
    {
        if (Time.time - gotHitState.EntryTime <= _playerStats.InvulnerableTime && _isVunerable)
            gameObject.layer = LayerMask.NameToLayer(IGNORE_ENEMIES_LAYER);
        else if (_state is not DashState)
        {
            _isVunerable = false;
            gameObject.layer = LayerMask.NameToLayer(PLAYER_LAYER);
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

            //Nếu ở rất gần vị trí trò chuyện thì return 0 cần flip tránh bug
            if (Mathf.Abs(transform.position.x - _interactPosition.x) < NEAR_CONVERSATION_RANGE)
                return;

            if (isFacingRight && transform.position.x > InteractPosition.x + CAN_START_CONVERSATION_RANGE)
            {
                FlippingSprite();
                //Debug.Log("Flip to Left");
            }
            else if (!isFacingRight && transform.position.x < InteractPosition.x - CAN_START_CONVERSATION_RANGE)
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
            //Nếu ở rất gần vị trí trò chuyện thì switch sang Idle luôn tránh bug chạy lung tung ở Run
            if (Mathf.Abs(transform.position.x - _interactPosition.x) < NEAR_CONVERSATION_RANGE)
                ChangeState(idleState);
            else
            {
                Debug.Log("run");
                Invoke(nameof(ChangeToRun), DELAY_PLAYER_RUN);
            }
        }
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
        GroundAndWallCheck();
        SweptForCoin();
    }

    private void SweptForCoin()
    {
        if (_isMagnetized)
        {
            _arrCoinsMagnetized = Physics2D.OverlapCircleAll(transform.position, MAGNETIC_BUFF_RADIUS, _coinLayer);
            for (int i = 0; i < _arrCoinsMagnetized.Length; i++)
            {
                if (!_hashCoinsMagnetized.Contains(_arrCoinsMagnetized[i]))
                {
                    _hashCoinsMagnetized.Add(_arrCoinsMagnetized[i]);
                    _arrCoinsMagnetized[i].GetComponent<Coin>().MoveTowardPlayer();
                }
            }
            Debug.Log("magg");
        }
    }

    private void OnDrawGizmos()
    {
        //Draw Ground, Magnetic Check
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        //Gizmos.DrawSphere(transform.position, MAGNETIC_BUFF_RADIUS);

        //Draw Wall Check
        if (isFacingRight)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        else
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void HandleInput()
    {
        if (_hasWinGame) return;

        if (_joystick.Horizontal > 0.2f)
            dirX = 1f;
        else if (_joystick.Horizontal < -0.2f)
            dirX = -1f;
        else
            dirX = 0f;

        //dirY = _joystick.Vertical;
        //Debug.Log("ver: " + dirY);
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
        //Ở trên Platform thì 0 cần GCheck tránh bug
        if (!_isOnPlatform)
            _canJump = isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallLayer);
        else
            _canJump = true; //done bug
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
        //bug here, do dính gothit ngoài manager
        //Vứt trong Dash Animation để Delay việc Update của Dash 
    }

    public void HandleDeadState()
    {
        //Tránh func này bị gọi nhiều lần dù đã chết
        //VD: Dính dmg từ quái khi bay màu dẫn đến loadscene loạn xạ
        if (!_hasDead)
            _hasDead = true;
        else
            return;

        if (_isCursed)
        {
            PlayerHealthManager.Instance.HandleIfCurse();
            HandleDeadAnimation();
            EventsManager.Instance.NotifyObservers(EEvents.OnLevelCompleted, ELevelResult.Failed);
        }
        else
        {
            //Check vì có thể dính DeadZone nên 0 vào state GotHit
            if (PlayerHealthManager.Instance.CurrentHP > 0)
            {
                PlayerHealthManager.Instance.ChangeHPState(HP_STATE_LOST);
                if (PlayerHealthManager.Instance.CurrentHP == 0)
                {
                    EventsManager.Instance.NotifyObservers(EEvents.OnLevelCompleted, ELevelResult.Failed);
                }
                else
                    UIManager.Instance.AnimateAndTransitionScene(SceneManager.GetActiveScene().buildIndex, false, true);
            }
            else
                EventsManager.Instance.NotifyObservers(EEvents.OnLevelCompleted, ELevelResult.Failed);

            HandleDeadAnimation();
        }
    }

    private void HandleDeadAnimation()
    {
        anim.SetTrigger(DEAD_ANIMATION);
        rb.bodyType = RigidbodyType2D.Static;
        _capCollider2D.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Enemies"); //Đổi layer tránh bị quái Detect dù đã chết
        SoundsManager.Instance.PlaySfx(ESoundName.PlayerDeadSfx, 1.0f);
    }

    private void HandleCoyoteTime()
    {
        if (!isOnGround && !_hasStart)
        {
            _hasStart = true;
            _startCoyote = Time.time;
            //Bấm giờ khi 0 thấy Ground nữa
        }
        else if (isOnGround)
            _hasStart = false; //Thấy Ground lại r thì reset

        //Đã bấm giờ và đang trong coyoteTime thì vẫn đc Jump
        if (Time.time - _startCoyote <= _playerStats.CoyoteTime && _hasStart)
            _canJump = true;
        else if (Time.time - _startCoyote > _playerStats.CoyoteTime && _hasStart)
            _canJump = false;
    }

    private void HandleCollideGround()
    {
        isOnGround = true;
        _canDbJump = true; //Player chạm đất thì mới cho DbJump tiếp
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
        //Debug.Log("Enable");
    }

    /// <summary>
    ///2 hàm dưới để xử lý màu alpha khi player dính đòn  
    /// </summary>

    private IEnumerator Twinkling()
    {
        //Lock - Đảm bảo chỉ gọi coroutine sau khi đi đc 1 vòng Alpha Value:
        //từ 1 -> AlphaVal -> 1
        _hasStartCoroutine = true;
        _spriteRenderer.color = new Color(1f, 1f, 1f, _playerStats.AlphaValueGotHit);
        //Debug.Log("tang lan: " + _count);

        yield return new WaitForSeconds(_playerStats.TimeEachApplyAlpha);

        //Thêm check đây nữa 
        if (BuffsManager.Instance.GetBuff(EBuffs.Invisible).IsActivating)
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
        if (BuffsManager.Instance.GetBuff(EBuffs.Invisible).IsActivating)
            return;

        if (Time.time - gotHitState.EntryTime <= _playerStats.InvulnerableTime && !_hasStartCoroutine && _isApplyGotHitEffect)
            StartCoroutine(Twinkling());
        else if (Time.time - gotHitState.EntryTime > _playerStats.InvulnerableTime)
        {
            //Hết thgian miễn dmg r thì trả màu về như cũ cho nó
            //NẾU trên ng 0 có buff vô hình, còn có thì return và set lại bool
            if (BuffsManager.Instance.GetBuff(EBuffs.Invisible).IsActivating)
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
        GameObject dEff = Pool.Instance.GetObjectInPool(EPoolable.Dashable);
        dEff.SetActive(true);
        //Event của Dash animation
        //Dùng để ra dấu hiệu chỉ đc dash khi hết effect
    }

    private void PushBack(object obj)
    {
        PushBackInfor pInfo = (PushBackInfor)obj;
        if (rb)
            rb.AddForce(pInfo.IsPushFromRight ? pInfo.PushForce * new Vector2(-1f, 1f) : pInfo.PushForce);
        else
            Debug.Log("RB Player nULL");
    }

    private void UnlockSkill(object obj)
    {
        SpecialItemStaticData sItemSData = (SpecialItemStaticData)obj;
        switch (sItemSData.Ability.AbilityName)
        {
            case ESkills.DoubleJump:
                UnlockedDbJump = true; 
                break;
            case ESkills.WallSlide:
                UnlockedWallSlide = true;
                break;
            case ESkills.Dash:
                _unlockedDash = true;
                break;
        }
    }

    private void UpdateRespawnPosition(object obj)
    {
        Vector3 checkPointPos = (Vector3)obj;

        string keyX = ESpecialStates.PlayerPositionUpdatedX.ToString();
        string keyY = ESpecialStates.PlayerPositionUpdatedY.ToString();
        string keyZ = ESpecialStates.PlayerPositionUpdatedZ.ToString();

        PlayerPrefs.SetFloat(keyX, checkPointPos.x);
        PlayerPrefs.SetFloat(keyY, checkPointPos.y);
        PlayerPrefs.SetFloat(keyZ, checkPointPos.z);
        PlayerPrefs.Save();

        GameManager.Instance.ListPrefsInconsistentKeys.Add(keyX);
        GameManager.Instance.ListPrefsInconsistentKeys.Add(keyY);
        GameManager.Instance.ListPrefsInconsistentKeys.Add(keyZ);
    }

    private void HandleWinGame(object obj)
    {
        _hasWinGame = true;
    }

    private void ValidateBuffs(object obj)
    {
        List<Skills> listSks = ToggleAbilityItemHelper.GetListActivatedSkills();
        Skills speed = listSks.Find(x => x.SkillName == ESkills.FasterSpeed);
        Skills jump = listSks.Find(x => x.SkillName == ESkills.HigherJump);
        Skills magnetic = listSks.Find(x => x.SkillName == ESkills.Magnetic);
        Skills curse = listSks.Find(x => x.SkillName == ESkills.Curse);
        Skills bountyHunter = listSks.Find(x => x.SkillName == ESkills.BountyHunter);

        MoveSpeed = (speed != null) ? _playerStats.SpeedX * SPEED_BUFF_FACTOR : _playerStats.SpeedX;
        JumpSpeed = (jump != null) ? _playerStats.SpeedY * JUMP_BUFF_FACTOR : _playerStats.SpeedY;
        _isMagnetized = (magnetic != null);
        _isCursed = (curse != null);
        if (_isMagnetized)
        {
            EventsManager.Instance.NotifyObservers(EEvents.OnBountyMarked);
            Debug.Log("bounty noti");
        }
        if (_isCursed)
        {
            EventsManager.Instance.NotifyObservers(EEvents.OnBeingCursed);
            Debug.Log("curse noti");
        }
        Debug.Log("vali buff");
    }
}