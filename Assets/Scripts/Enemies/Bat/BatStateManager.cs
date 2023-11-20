using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatStateManager : MonoBehaviour
{
    //Tạo effect sáng tối để có chỗ để mấy con này trong chỗ ánh sáng thấp
    //Nếu đi xa quá đến chỗ có ánh sáng mạnh thì quay đầu về ngủ
    //Match Bat's logic :)
    //Còn xác định Range phát hiện player

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

    [Header("Left Right Boundaries")]
    [SerializeField] private Transform maxPointLeft;
    [SerializeField] private Transform maxPointRight;

    [Header("Speed")]
    [SerializeField] private float chaseSpeed;

    [Header("Time")]
    [SerializeField] private float sleepTime;
    [SerializeField] private float restTime;

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public Animator GetAnimator() { return this.anim; }

    public float GetSleepTime() { return this.sleepTime; }

    public float GetRestTime() { return this.restTime; }

    public float GetChaseSpeed() { return this.chaseSpeed; }

    public bool GetIsFacingRight() { return this.isFacingRight; }

    public Transform GetPlayer() { return this.player; }

    public Transform GetSleepPos() { return this.sleepPos; }

    public Transform GetMaxPointLeft() { return this.maxPointLeft; }

    public Transform GetMaxPointRight() { return this.maxPointRight; }

    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
