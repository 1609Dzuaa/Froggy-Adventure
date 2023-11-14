using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomStateManager : BaseStateManager
{
    //Sinh vật vô hại, tương tự như nấm nhỏ trong HK :)
    //Làm 1 cái Ray check player dài từ trái qua phải luôn
    //Với các GameObject có từ 4 states trở lên thì dùng StateManager để quản lý

    public MrIdleState mrIdleState = new();
    public MrWalkState mrWalkState = new();
    public MrRunState mrRunState = new();
    public MrGotHitState mrGotHitState = new();

    [Header("Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("Force")]
    [SerializeField] private Vector2 knockForce;

    [Header("Player Check")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float checkDistance = 50.0f;
    [SerializeField] private LayerMask playerLayer;
    private bool hasDetectedPlayer = false;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 3.0f;
    [SerializeField] private LayerMask wallLayer;
    private bool hasCollidedWall = false;

    [Header("Safe Check")]
    [SerializeField] private Transform safeCheck;
    [SerializeField] private float safeCheckDistance = 50.0f;
    private bool isDetected = false; //bị phát hiện

    [Header("Time")]
    [SerializeField] private float restDuration;
    [SerializeField] private float walkDuration; //Sao kh dùng Time cho con Rhino :?
    [SerializeField] private float runDelay;
    [SerializeField] private float gotHitDuration; //Khoảng thgian GotHit trước khi bị destroy hẳn

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isFacingRight = false;
    private int changeRightDirection;
    private bool hasGotHit = false;

    public void SetChangeRightDirection(int para) { changeRightDirection = para; }

    public bool GetHasDetectedPlayer() { return hasDetectedPlayer; }

    public bool GetIsDetected() { return isDetected; }

    public bool GetHasCollidedWall() { return this.hasCollidedWall; }

    public float GetRestDuration() { return this.restDuration; }

    public float GetWalkDuration() { return this.walkDuration; }

    public float GetRunDelay() { return this.runDelay; }

    public float GetWalkSpeed() { return this.walkSpeed; }

    public float GetRunSpeed() { return this.runSpeed; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public Vector2 GetKnockForce() { return this.knockForce; }

    public BoxCollider2D GetBoxCollider2D() { return this.boxCollider; }

    public float GetGotHitDuration() { return this.gotHitDuration; }

    public Rigidbody2D GetRigidBody2D() { return rb; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        state = mrIdleState;
        state.EnterState(this);
    }

    public override void ChangeState(BaseState state)
    {
        //Got Hit thì return 0 cho chuyển state nữa (Chết là hết)
        if (this.state is MrGotHitState)
            return;

        this.state.ExitState();
        this.state = state;
        this.state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        state.UpdateState();
        DetectWall();
        DetectPlayer();
        //Debug.Log("CollidedWall: " + wallCheck.position.x);
    }

    private void FixedUpdate()
    {
        state.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !hasGotHit)
        {
            hasGotHit = true;
            var playerStateManager = collision.gameObject.GetComponent<PlayerStateManager>();
            playerStateManager.GetRigidBody2D().AddForce(new Vector2(0f, 700f));
            ChangeState(mrGotHitState);
        }
    }

    public void FlippingSprite()
    {
        //Maybe here
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    private void DetectPlayer()
    {
        if (!isFacingRight)
        {
            hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.left, checkDistance, playerLayer);
            isDetected = Physics2D.Raycast(new Vector2(safeCheck.position.x, safeCheck.position.y), Vector2.right, safeCheckDistance, playerLayer);
        }
        else
        {
            hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.right, checkDistance, playerLayer);
            isDetected = Physics2D.Raycast(new Vector2(safeCheck.position.x, safeCheck.position.y), Vector2.left, safeCheckDistance, playerLayer);
        }

        DrawRayDetectPlayer();
    }

    private void DetectWall()
    {
        //hasCollidedWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f);
        if (!isFacingRight)
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.left, wallCheckDistance, wallLayer);
        else
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.right, wallCheckDistance, wallLayer);
    }

    private void DrawRayDetectPlayer()
    {
        if (isFacingRight)
            Debug.DrawRay(wallCheck.position, Vector2.right * wallCheckDistance, Color.green);
        else
            Debug.DrawRay(wallCheck.position, Vector2.left * wallCheckDistance, Color.green);
        /*if (hasDetectedPlayer)
        {
            if (!isFacingRight)
            {
                Debug.DrawRay(playerCheck.position, Vector2.left * checkDistance, Color.red);
            }
            else
            {
                Debug.DrawRay(playerCheck.position, Vector2.right * checkDistance, Color.red);
            }
        }
        else
        {
            if (!isFacingRight)
            {
                Debug.DrawRay(playerCheck.position, Vector2.left * checkDistance, Color.green);
            }
            else
            {
                Debug.DrawRay(playerCheck.position, Vector2.right * checkDistance, Color.green);
            }
        }
        if(isDetected)
        {
            if (!isFacingRight)
            {
                Debug.DrawRay(safeCheck.position, Vector2.right * checkDistance, Color.red);
            }
            else
            {
                Debug.DrawRay(safeCheck.position, Vector2.left * safeCheckDistance, Color.red);
            }
        }
        else
        {
            if (!isFacingRight)
            {
                Debug.DrawRay(safeCheck.position, Vector2.right * safeCheckDistance, Color.green);
            }
            else
            {
                Debug.DrawRay(safeCheck.position, Vector2.left * safeCheckDistance, Color.green);
            }
        }*/
    }

    private void AllowRunFromPlayer()
    {
        FlippingSprite(); //Quay đầu chạy khỏi Player
        ChangeState(mrRunState);
    }

    private void AllowWalk1()
    {
        HandleChangeDirection();
        ChangeState(mrWalkState);
        //Use for Invoke in MrIdleState
    }

    private void AllowWalk2()
    {
        ChangeState(mrWalkState);
        //Use for Invoke in MrIdleState
    }

    private void HandleChangeDirection()
    {
        if (changeRightDirection == 1 && !isFacingRight)
            FlippingSprite();
        else if (changeRightDirection == 0 && isFacingRight)
            FlippingSprite();
        //Random change direction
    }

    private void AllowUpdateMrRunState()
    {
        mrRunState.SetTrueUpdateMrRunState();
    }

    private void SetTrueGotHitUpdate()
    {
        mrGotHitState.SetTrueAllowUpdate();
    }

    public void DestroyItSelf()
    {
        Destroy(this.gameObject);
    }

}
