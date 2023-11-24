using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyStateManager : MonoBehaviour
{
    [Header("Distance")]
    [SerializeField] private float checkJumpDistance;

    [Header("Speed")]
    [SerializeField] private float patrolSpeed = 1.0f;
    [SerializeField] private float runSpeed = 5.0f;

    //Nên bung lực nhảy tuỳ vào khcach với player cho hợp lý
    [Header("Force")]
    [SerializeField] private Vector2 jumpForce;

    [Header("Time")]
    [SerializeField] private float patrolTime;
    [SerializeField] private float restTime;
    [SerializeField] private float jumpDelay = 0.25f;
    [SerializeField] private float idleDelay = 0.25f;

    [Header("Player Check")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private GameObject player;
    [SerializeField] private float checkDistance = 50.0f;
    [SerializeField] private LayerMask playerLayer;
    private bool hasDetectedPlayer = false;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 3.0f;
    [SerializeField] private LayerMask wallLayer;
    private bool hasCollidedWall = false;

    private BunnyBaseState _state;
    public BunnyIdleState bunnyIdleState = new();
    public BunnyPatrolState bunnyPatrolState = new();
    public BunnyJumpState bunnyJumpState = new();
    public BunnyFallState bunnyFallState = new();
    public BunnyGotHitState bunnyGotHitState = new();

    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = false;

    public Rigidbody2D GetRigidBody2D() { return this.rb; } 

    public Animator GetAnimator() { return this.anim; }

    public Vector2 GetJumpForce() { return this.jumpForce; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public bool GetHasDetectPlayer() { return this.hasDetectedPlayer; }

    public bool GetHasCollidedWall() {  return this.hasCollidedWall; }

    public float GetRunSpeed() { return this.runSpeed; }

    public float GetJumpDelay() { return this.jumpDelay; }

    public float GetIdleDelay() { return this.idleDelay; }

    public float GetPatrolSpeed() { return this.patrolSpeed; }

    public float GetRestTime() { return this.restTime; }

    public float GetPatrolTime() { return this.patrolTime; }

    public float GetCheckJumpDistance() { return this.checkJumpDistance; }

    public GameObject GetPlayer() { return this.player; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (transform.eulerAngles.y == 180f)
            isFacingRight = true;
        _state = bunnyIdleState;
        _state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();
        DetectPlayer();
        DetectWall();
        Debug.Log("velo y: " + rb.velocity.y);
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    public void ChangeState(BunnyBaseState state)
    {
        _state.ExitState();
        _state = state;
        _state.EnterState(this);
    }

    private void DetectPlayer()
    {
        if (!isFacingRight)
            hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.left, checkDistance, playerLayer);
        else
            hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.right, checkDistance, playerLayer);
        DrawRayDetectPlayer();
    }

    private void DrawRayDetectPlayer()
    {
        if (hasDetectedPlayer)
            Debug.DrawRay(playerCheck.position, -1 * transform.right * checkDistance, Color.red);
        else
            Debug.DrawRay(playerCheck.position, -1 * transform.right * checkDistance, Color.green);
    }

    private void DetectWall()
    {
        if (!isFacingRight)
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.left, wallCheckDistance, wallLayer);
        else
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.right, wallCheckDistance, wallLayer);
    }

    public void FlippingSprite()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    private void JumpDelay()
    {
        ChangeState(bunnyJumpState);
    }

    private void IdleDelay()
    {
        ChangeState(bunnyIdleState);
    }
}
