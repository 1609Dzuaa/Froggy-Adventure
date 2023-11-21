using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatStateManager : MonoBehaviour
{
    //Tạo effect sáng tối để có chỗ để mấy con này trong chỗ ánh sáng thấp
    //Nếu đi xa quá đến chỗ có ánh sáng mạnh thì quay đầu về ngủ
    //Match Bat's logic :)
    //Vẫn còn bug con bat có thể đè player xuyên qua map :v

    private BatBaseState _state;
    public BatSleepState batSleepState = new();
    public BatIdleState batIdleState = new();
    public BatCeilInState batCeilInState = new();
    public BatCeilOutState batCeilOutState = new();
    public BatFlyState batFlyState = new();
    public BatChaseState batChaseState = new();
    public BatRetreatState batRetreatState = new();
    public BatGotHitState batGotHitState = new();

    [Header("Object To Detect")]
    [SerializeField] private Transform player; //Dùng để xác định và đuổi theo player
    [SerializeField] private Transform sleepPos; //Dùng để xác định và về chỗ ngủ

    [Header("Boundaries")]
    [SerializeField] private Transform maxPointLeft;
    [SerializeField] private Transform maxPointRight;
    [SerializeField] private float attackRange; //trigger bat's chase state

    [Header("Parent")]
    [SerializeField] private GameObject parent; //Lấy parent của con bat để Destroy

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float degreeEachRotation;
    [SerializeField] private float timeEachRotate;

    [Header("Speed")]
    [SerializeField] private Vector2 flySpeed;
    [SerializeField] private float chaseSpeed;

    [Header("Time")]
    [SerializeField] private float sleepTime;
    [SerializeField] private float flyTime;
    [SerializeField] private float restTime;

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public Animator GetAnimator() { return this.anim; }

    public CapsuleCollider2D GetCapsuleCollider2D() { return this.collider2D; }

    public float GetSleepTime() { return this.sleepTime; }

    public float GetFlyTime() { return this.flyTime; }

    public float GetRestTime() { return this.restTime; }

    public Vector2 GetFlySpeed() { return this.flySpeed; }

    public float GetChaseSpeed() { return this.chaseSpeed; }

    public float GetTimeEachRotate() { return this.timeEachRotate; }

    public float GetDegreeEachRotation() { return this.degreeEachRotation; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public float GetAttackRange() { return this.attackRange; }

    public Transform GetPlayer() { return this.player; }

    public Transform GetSleepPos() { return this.sleepPos; }

    public Transform GetMaxPointLeft() { return this.maxPointLeft; }

    public Transform GetMaxPointRight() { return this.maxPointRight; }

    private Rigidbody2D rb;
    private Animator anim;
    private new CapsuleCollider2D collider2D;
    private bool isFacingRight = false;
    private bool hasGotHit = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2D = GetComponent<CapsuleCollider2D>();
        _state = batSleepState;
        _state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _state.UpdateState();
    }

    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player" && !hasGotHit)
        {
            hasGotHit = true;
            var pSM = collision.gameObject.GetComponent<PlayerStateManager>();
            pSM.GetRigidBody2D().AddForce(pSM.GetJumpOnEnemiesForce());
            ChangState(batGotHitState);
        }
    }

    public void ChangState(BatBaseState state)
    {
        if (_state is BatGotHitState)
            return;
        _state.ExitState();
        _state = state;
        _state.EnterState(this);
    }

    //CeilOut Animation's Event
    public void Idle()
    {
        ChangState(batIdleState);
    }

    //Idle Animation's Event
    public void AllowUpdateIdle()
    {
        batIdleState.SetTrueAllowUpdate();
    }

    //Ceil In Animation's Event
    public void Sleep()
    {
        ChangState(batSleepState);
    }

    //Got Hit Animation's Event
    public void SelfDestroy()
    {
        Destroy(parent);
    }

    public void FlipLeft()
    {
        isFacingRight = false;
        transform.Rotate(0, 180, 0);
    }

    public void FlipRight()
    {
        isFacingRight = true;
        transform.Rotate(0, 180, 0);
    }
}
