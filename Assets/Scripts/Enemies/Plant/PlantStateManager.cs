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

    [Header("Bullet")]
    [SerializeField] private Transform bullet;

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

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public Animator GetAnimator() { return this.anim; }

    public bool GetHasDetectedPlayer() { return this.hasDetectedPlayer; }

    public float GetTimeEachRotate() { return this.timeEachRotate; }

    public float GetDegreeEachRotation() {  return this.degreeEachRotation; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
        _state = plantIdleState;
        _state.EnterState(this);
    }

    void Update()
    {
        DetectPlayer();
        _state.UpdateState();
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
        hasDetectedPlayer = Physics2D.Raycast(new Vector2(playerCheck.position.x, playerCheck.position.y), Vector2.left, checkDistance, playerLayer);
        DrawRayDetectPlayer();
    }

    private void DrawRayDetectPlayer()
    {
        if (hasDetectedPlayer)
            Debug.DrawRay(playerCheck.position, Vector2.left * checkDistance, Color.red);
        else
            Debug.DrawRay(playerCheck.position, Vector2.left * checkDistance, Color.green);
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
        //Cho th bullet nhận th này làm parent, tự đo ra đc các số dưới
        Vector3 spawnPos = new Vector3(transform.position.x - 1.34f, transform.position.y + 0.15f, transform.position.z);
        Instantiate(bullet, spawnPos, Quaternion.identity, null);
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
