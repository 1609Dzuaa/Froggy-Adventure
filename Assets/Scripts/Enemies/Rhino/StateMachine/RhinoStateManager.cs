using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RhinoStateManager : MonoBehaviour
{
    private RhinoBaseState _state;
    public RhinoIdleState rhinoIdleState = new();
    public RhinoRunState rhinoRunState = new();
    public RhinoWallHitState rhinoWallHitState = new();
    public RhinoGotHitState rhinoGotHitState = new();
    public RhinoPatrolState rhinoPatrolState = new();

    [Header("Speed")]
    [SerializeField] private float patrolSpeed = 1.0f;
    [SerializeField] private float runSpeed = 5.0f;

    [Header("Spawn Warning")]
    [SerializeField] private GameObject _warning;
    [SerializeField] private float spawnDistanceY;

    [Header("Force")]
    [SerializeField] private float knockUpForce = 5.0f;
    [SerializeField] private float knockLeftForce = 5.0f;

    [Header("Player Check")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float checkDistance = 50.0f;
    [SerializeField] private float chasingDelay = 0.25f;
    [SerializeField] private LayerMask playerLayer;
    private bool hasDetectedPlayer = false;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 3.0f;
    [SerializeField] private LayerMask wallLayer;
    private bool hasCollidedWall = false;

    [Header("Patrol Field")]
    [SerializeField] private float patrolDistance = 10.0f;
    [SerializeField] private float restDuration = 1.0f; //Thời gian idle sau khi patrol xong

    //Private Field
    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = false;
    private new BoxCollider2D collider;
    private int changeRightDirection;
    private bool hasGotHit = false;

    //Public Func
    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public Animator GetAnimator() { return this.anim; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public float GetRunSpeed() { return this.runSpeed; }

    public float GetPatrolSpeed() { return this.patrolSpeed; }

    public float GetPatrolDistance() { return this.patrolDistance; }

    public float GetRestDuration() { return this.restDuration; }

    public bool GetHasDetectedPlayer() { return this.hasDetectedPlayer; }

    public bool GetHasCollidedWall() { return this.hasCollidedWall; }

    public float GetChasingDelay() { return this.chasingDelay; }

    public BoxCollider2D GetBoxCollider2D() { return this.collider; }

    //SetFunc

    public void SetChangeRightDirection(int para) { this.changeRightDirection = para; } 

    public Vector2 GetKnockForce() { return new Vector2(-1 * this.knockLeftForce, this.knockUpForce); }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        _state = rhinoIdleState;
        _state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();
        DetectPlayer();
        DetectWall();
        //Debug.Log("Right?: " + changeRightDirection); //
        //Xử lý thêm nếu bị Player đụng từ đằng sau thì quay lại tông Player (should we ?)
        //Thêm sprite cảnh báo(vàng đen) nếu detected đc Player
        //Nếu tông Player thì tắt collider để nó có thể chạy xuyên qua
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    public void ChangeState(RhinoBaseState state)
    {
        //Got Hit thì return 0 cho chuyển state nữa (Chết là hết)
        if (this._state is RhinoGotHitState)
            return;

        this._state.ExitState();
        this._state = state;
        this._state.EnterState(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !hasGotHit)
        {
            hasGotHit = true;
            PlayerStateManager pSM = collision.GetComponent<PlayerStateManager>();
            pSM.GetRigidBody2D().AddForce(pSM.GetJumpOnEnemiesForce());
            ChangeState(rhinoGotHitState);
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

    private void DetectPlayer()
    {
        if (!isFacingRight)
            hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.left, checkDistance, playerLayer);
        else
            hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.right, checkDistance, playerLayer);

        DrawRayDetectPlayer();
    }

    private void DetectWall()
    {
        if (!isFacingRight)
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.left, wallCheckDistance, wallLayer);
        else
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.right, wallCheckDistance, wallLayer);
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

    public void FlippingSprite()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    private void SetTrueWallHitUpdate()
    {
        rhinoWallHitState.SetAllowUpdate();
    }

    private void SetTrueGotHitUpdate()
    {
        rhinoGotHitState.SetTrueAllowUpdate();
    }

    private void SetTruePatrolUpdate()
    {
        rhinoPatrolState.SetTrueAllowUpdate();
    }

    private void AllowChasingPlayer()
    {
        ChangeState(rhinoRunState);
        //Dùng để delay việc change state sau khi detected player và tông phải wall
    }

    private void AllowPatrol1()
    {
        HandleChangeDirection(); //Random trái phải để chọn hướng patrol
        ChangeState(rhinoPatrolState);
        //Hàm 1 thì có random chọn hướng để patrol
    }

    private void AllowPatrol2()
    {
        ChangeState(rhinoPatrolState);
        //Hàm 2 thì đơn giản là patrol theo hướng của isFacingRight
        //Dùng khi vừa đụng tường xong và muốn patrol theo hướng vừa flip sprite
    }

    private void HandleChangeDirection()
    {
        if (changeRightDirection == 1 && !isFacingRight)
            FlippingSprite();
        else if (changeRightDirection == 0 && isFacingRight)
            FlippingSprite();
        //Random change direction
    }

    public void DestroyItSelf()
    {
        Destroy(this.gameObject, 1f);
        //Destroy sau khi chết, tránh lãng phí tài nguyên
    }

    public void SpawnWarning()
    {
        Instantiate(_warning, new Vector3(transform.position.x, transform.position.y + spawnDistanceY, transform.position.z), Quaternion.identity, null);
    }

}
