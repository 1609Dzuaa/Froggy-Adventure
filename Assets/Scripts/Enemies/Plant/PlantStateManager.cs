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
    [SerializeField] private float bulletSpeed;

    private PlantBaseState _state;
    public PlantIdleState plantIdleState = new();
    public PlantAttackState plantAttackState = new();
    public PlantGotHitState plantGotHitState = new();

    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = false;

    public Rigidbody2D GetRigidBody2D() { return this.rb; }

    public Animator GetAnimator() { return this.anim; }

    public bool GetHasDetectedPlayer() { return this.hasDetectedPlayer; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

    public void FlippingSprite()
    {
        //Maybe here
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            ChangeState(plantGotHitState);
        }    
    }
}
