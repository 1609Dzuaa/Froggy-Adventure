using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    //Sinh vật vô hại, tương tự như nấm nhỏ trong HK :)

    [Header("Speed")]
    [SerializeField] private float runSpeed;
    private bool isRunning = false;

    [Header("Player Check")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float checkDistance = 50.0f;
    //[SerializeField] private float chasingDelay = 0.25f;
    [SerializeField] private LayerMask playerLayer;
    private bool hasDetectedPlayer = false;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 3.0f;
    [SerializeField] private LayerMask wallLayer;
    private bool hasCollidedWall;

    Rigidbody2D rb;
    Animator anim;
    bool isFacingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectWall();
        DetectPlayer();
        UpdateState();
        AnimationController();
    }

    private void FixedUpdate()
    {
        
    }

    public void FlippingSprite()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        //Hàm này dùng để lật sprite theo chiều ngang
    }

    private void DetectWall()
    {
        if (!isFacingRight)
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.left, wallCheckDistance, wallLayer);
        else
            hasCollidedWall = Physics2D.Raycast(new Vector2(wallCheck.position.x, wallCheck.position.y), Vector2.right, wallCheckDistance, wallLayer);
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

    private void AnimationController()
    {
        if(!isRunning)
            anim.SetInteger("state", 0);
        else if(isRunning)
            anim.SetInteger("state", 1);
    }
    
    private void UpdateState()
    {
        if (hasCollidedWall)
            FlippingSprite();
        if (hasDetectedPlayer)
        {
            FlippingSprite();
            isRunning = true;
        }
        if (isRunning && isFacingRight)
            rb.velocity = new Vector2(runSpeed, 0f);
        else if (isRunning && !isFacingRight)
            rb.velocity = new Vector2(-runSpeed, 0f);
        else
            rb.velocity = new Vector2(0f, 0f);
    }
}
