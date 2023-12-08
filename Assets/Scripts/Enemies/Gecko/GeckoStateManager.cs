using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoStateManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float restTime;
    [SerializeField] private float patrolTime;

    [Header("Speed")]
    [SerializeField] private float patrolSpeed;

    [Header("Player Check")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask playerLayer;
    private bool hasDetectedPlayer = false;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 3.0f;
    [SerializeField] private LayerMask wallLayer;
    //private bool hasCollidedWall = false;

    [Header("Teleport Distance")] //
    [SerializeField] float teleDistance;

    private GeckoBaseState _state;
    public GeckoIdleState geckoIdleState = new();
    public GeckoPatrolState geckoPatrolState = new();
    public GeckoHideState geckoHideState = new();
    public GeckoAttackState geckoAttackState = new();
    public GeckoGotHitState geckoGotHitState = new();

    public Rigidbody2D GetRigidBody2D() { return this.rb; }
    public Animator GetAnimator() { return this.anim; }

    public float GetRestTime() { return this.restTime; }

    public float GetPatrolTime() { return this.patrolTime; }

    public float GetPatrolSpeed() { return this.patrolSpeed; }

    public bool GetHasDetectPlayer() { return this.hasDetectedPlayer; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _state = geckoIdleState;
        _state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();
        DetectPlayer();
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    public void ChangeState(GeckoBaseState state)
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
        {
            if (!isFacingRight)
                Debug.DrawRay(playerCheck.position, Vector2.left * checkDistance, Color.red);
            else
                Debug.DrawRay(playerCheck.position, Vector2.right * checkDistance, Color.red);
        }
        else
        {
            if (!isFacingRight)
                Debug.DrawRay(playerCheck.position, Vector2.left * checkDistance, Color.green);
            else
                Debug.DrawRay(playerCheck.position, Vector2.right * checkDistance, Color.green);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x - checkDistance, playerCheck.position.y, playerCheck.position.z));

        if (!isFacingRight)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        else
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    public void FlippingSprite()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    //Hide Animation's Event
    public void Attack()
    {
        HandleTeleportAndFlipSprite();
        if (hasDetectedPlayer)
            ChangeState(geckoAttackState);
        else
            ChangeState(geckoIdleState);
    }

    private void HandleTeleportAndFlipSprite()
    {
        int isTeleport = Random.Range(0, 2); //0: 0 tele | 1: Tele ra đằng sau
        Debug.Log(isTeleport);
        Vector3 newPos;
        if (isTeleport > 0)
        {
            FlippingSprite();
            if (isFacingRight)
                newPos = new Vector3(playerPosition.position.x - teleDistance, transform.position.y, 0f);
            else
                newPos = new Vector3(playerPosition.position.x + teleDistance, transform.position.y, 0f);
        }
        else
            newPos = transform.position + Vector3.zero;
        transform.position = Vector2.Lerp(transform.position, newPos, 1f);
    }

    public void CheckAfterAttacked()
    {
        if (hasDetectedPlayer)
            ChangeState(geckoHideState);
        else
            ChangeState(geckoIdleState);
    }

    /*public void Withdraw()
    {
        if (isFacingRight)
            rb.AddForce(new Vector2(-100f, 100f));
        else
            rb.AddForce(new Vector2(100f, 100f));
    }

    /*private void Hide()
    {
        ChangeState(geckoHideState);
    }*/
}
