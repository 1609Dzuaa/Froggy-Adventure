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

    //Nên thêm giới hạn trái phải thay vì phụ thuộc vào địa hình
    //Khi Chase Player thì disable 2 th này ?
    [Header("Boundaries")]
    [SerializeField] private Transform maxPointLeft;
    [SerializeField] private Transform maxPointRight;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 3.0f;
    [SerializeField] private LayerMask wallLayer;
    private bool hasCollidedWall = false;

    [Header("Time")]
    [SerializeField] private float patrolTime;
    [SerializeField] private float restTime = 1.0f; //Thời gian idle sau khi patrol xong
    [SerializeField] private float restDelay; //Thgian delay idle sau khi 0 detect player lúc run

    [Header("Parent")]
    [SerializeField] private GameObject rhinoParent; //Use to Destroy

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

    public float GetPatrolTime() { return this.patrolTime; }

    public float GetRestTime() { return this.restTime; }

    public bool GetHasDetectedPlayer() { return this.hasDetectedPlayer; }

    public bool GetHasCollidedWall() { return this.hasCollidedWall; }

    public float GetChasingDelay() { return this.chasingDelay; }

    public float GetRestDelay() { return this.restDelay; }

    public BoxCollider2D GetBoxCollider2D() { return this.collider; }

    public Transform GetMaxPointLeft() { return this.maxPointLeft; }

    public Transform GetMaxPointRight() { return this.maxPointRight; }

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
        //if(detectedPlayer && state is not Run)
        //=>Change Run && CancelInvoke
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

    //Event của Wall Hit Animation
    private void SetTrueWallHitUpdate()
    {
        rhinoWallHitState.SetAllowUpdate();
        //Mục đích chỉ là cho chạy hết animation r mới cho update ở state WH
    }

    //Event của GotHit Animation
    private void SetTrueGotHitUpdate()
    {
        rhinoGotHitState.SetTrueAllowUpdate();
    }
    
    //Event của Patrol Animation
    private void SetTruePatrolUpdate()
    {
        rhinoPatrolState.SetTrueAllowUpdate();
        //Delay 1 khoảng ngắn sau khi vào state patrol để
        //tránh tình trạng quay mặt rồi run ngay lập tức!
    }

    //Dùng để Invoke khi detected player
    private void AllowChasingPlayer()
    {
        ChangeState(rhinoRunState);
        //Dùng để delay việc change state sau khi detected player và tông phải wall
    }

    private void ChangeToIdle()
    {
        ChangeState(rhinoIdleState);
    }

    public void DestroyItSelf()
    {
        Destroy(rhinoParent, 1f);
        //Destroy sau khi chết, tránh lãng phí tài nguyên
    }

    public void SpawnWarning()
    {
        Instantiate(_warning, new Vector3(transform.position.x, transform.position.y + spawnDistanceY, transform.position.z), Quaternion.identity, null);
    }

}
