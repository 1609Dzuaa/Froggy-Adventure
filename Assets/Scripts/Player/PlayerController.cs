using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Class này để quản lý các thuộc tính của nhân vật, va chạm,...

    private int numOrange = 0;
    private float dirX, dirY;
    private bool isOnGround;
    private bool isOnPlatform;
    private Rigidbody2D rb; //use for control Rb
    private StateManager stateManager; //dùng để truy cập và chạy animations

    //SerializeField:
    //Cho phép điều chỉnh ngoài Editor mà 0 làm mất tính đóng gói
    [SerializeField] private float vX, vY, dashSpeed;
    [SerializeField] private Text txtScore;
    [SerializeField] private AudioSource JumpSound;
    [SerializeField] private AudioSource CollectSound;
    [SerializeField] private AudioSource DeadSound;

    //Func
    public Rigidbody2D GetRigidbody2D() { return this.rb; }

    public bool GetIsOnGround() { return this.isOnGround; }
    
    public void SetIsOnGround(bool value) { this.isOnGround = value; }
    
    public float GetvX() { return this.vX; }

    public float GetvY() { return this.vY; }

    public float GetDirX() { return this.dirX; }

    public float GetDirY() { return this.dirY; }

    public AudioSource GetAudioSource() { return this.JumpSound; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateManager = GetComponent<StateManager>();

        //TASK:
        //Improve Scene 1 and Complete Scene 2
        //Fixing moving platform horizontal "bug" ? - Try other approach
        //Adding Sliding Wall State, Dash ...
        //...
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");

        /*if(Input.GetKeyDown(KeyCode.E))
        {
            stateManager.ChangeState(stateManager.dashState);
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isOnGround = true;

        if (collision.collider.CompareTag("Trap"))
        {
            stateManager.GetAnimator().SetTrigger("dead");
            rb.bodyType = RigidbodyType2D.Static;
            DeadSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orange"))
        {
            numOrange++;
            txtScore.text = "Oranges: " + numOrange.ToString();
            Destroy(collision.gameObject);
            CollectSound.Play();
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.SetParent(collision.gameObject.transform);
            isOnGround = true;
        }

        if (collision.gameObject.name == "Checkpoint")
        {
            //Reload();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Platform"))
        {
            this.transform.SetParent(null); 
            isOnGround = false;
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Nên tạo thêm class GameController và vứt thg này vào trong đó
    }

}
