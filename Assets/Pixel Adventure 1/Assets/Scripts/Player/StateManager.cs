using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    //Class này chỉ dùng để quản lý State và Animation tương ứng
    private BaseState state;
    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    //2 States dưới đụng sau
    //public WallJumpState wallJumpState = new WallJumpState();
    //public DashState dashState = new DashState();

    private Animator anim;  //use for control animation
    private SpriteRenderer sprite;  //use for control sprite
    private PlayerController playerController;

    //Public Field
    //Tạo các enum của State để gán giá trị tương ứng cho Animations
    public enum EnumState { idle, walk, jump, fall, walljump, dash }

    public Animator GetAnimator() { return this.anim; }

    public SpriteRenderer GetSpriteRenderer() { return this.sprite; }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        state = idleState;
        state.EnterState(this, playerController);
    }

    // Update is called once per frame
    void Update()
    {
        state.UpdateState(this, playerController);
    }

    private void FixedUpdate()
    {
        state.FixedUpdate(this, playerController);
    }

    public void ChangeState(BaseState state)
    {
        this.state = state;
        state.EnterState(this, playerController);
    }

}