using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class PlantStateManager : MonoBehaviour
{
    [Header("Player Check")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float checkDistance = 50.0f;
    [SerializeField] private LayerMask playerLayer;
    private bool hasDetectedPlayer = false;

    [Header("Bullet & Shoot Pos")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPosition;

    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float degreeEachRotation;
    [SerializeField] private float timeEachRotate;

    private PlantBaseState _state;
    public PlantIdleState plantIdleState = new();
    public PlantAttackState plantAttackState = new();
    public PlantGotHitState plantGotHitState = new();

    private Rigidbody2D rb;
    private Animator anim;
    private new BoxCollider2D collider2D;
    private bool hasGotHit = false;
    private bool isFacingRight = false;

    public Animator GetAnimator() { return this.anim; }

    public bool GetHasDetectedPlayer() { return this.hasDetectedPlayer; }

    public float GetTimeEachRotate() { return this.timeEachRotate; }

    public float GetDegreeEachRotation() {  return this.degreeEachRotation; }

    private void Awake()
    {
        //Đụng sau
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
        if (transform.rotation.eulerAngles.y == 180f)
            isFacingRight = true;
        //Debug.Log("IfR: " + transform.right);
        _state = plantIdleState;
        _state.EnterState(this);
    }

    void Update()
    {
        DetectPlayer();
        _state.Update();
        //Debug.Log("IFR: " + isFacingRight);
    }

    public void ChangeState(PlantBaseState state)
    {
        if (_state is PlantGotHitState)
            return;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !hasGotHit)
        {
            hasGotHit = true; //Make sure only trigger and addforce once
            PlayerStateManager pSM = collision.GetComponent<PlayerStateManager>();
            //Vấn đề là nếu jump và fall bthg lên đầu enemy thì addforce hoạt động tốt
            //Còn nếu Dbjump và fall lên đầu enemy thì addforce chỉ nhích đc 1 tí
            //Do Dbjump => Try AddForce when jump ?
            pSM.GetRigidBody2D().AddForce(pSM.GetJumpOnEnemiesForce());
            ChangeState(plantGotHitState);
        }    
    }

    //Event Func in Attack Animation
    private void SpawnBullet()
    {
        //Xài cách này tha hồ điều chỉnh ngoài Inspector, hạn chế hard - coded

        //Mỗi lần bắn, tạo 1 viên đạn mới và set hướng cho nó
        //= chính hướng mặt hiện tại của cây(để điều chỉnh vector vận tốc)
        //Cách cũ: Instantiate(bullet, shootPosition.position, transform.rotation);
        //Là mình chỉ tạo bản sao của cái bullet(lúc này isDirectionRight của nó 
        //mặc định là false) dẫn đến việc vector vận tốc của bullet hđ 0 như ý
        GameObject _bullet;
        _bullet = Instantiate(bullet, shootPosition.position, transform.rotation);
        _bullet.GetComponent<BulletController>().SetIsDirectionRight(isFacingRight);
    }

    //Event Func in Attack Animation
    private void HandleGotHit()
    {
        collider2D.enabled = false;
        Invoke("SelfDestroy", 1.5f);
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
