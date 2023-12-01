using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [Header("Break Piece")]
    [SerializeField] private Transform brPiece1;
    [SerializeField] private Transform brPiece2;
    [SerializeField] private Transform brPiece3;
    [SerializeField] private Transform brPiece4;

    [Header("Mystery Gift")] //Random 1 trong các gift sau - có thể thêm enemy nhỏ
    [SerializeField] private Transform apple;
    [SerializeField] private Transform banana;
    [SerializeField] private Transform cherry;
    [SerializeField] private Transform orange;
    [SerializeField] private Transform mushroom;

    [Header("Force Apply To Player")]
    [SerializeField] private float forceApply; //Force apply to player when jump on this
    //Có bug(?) khi nhảy lên box và bấm nhảy tiếp thì bật rất cao! @@
    //Chỉnh lại sound

    [Header("Sound")]
    [SerializeField] private AudioSource brokeSound;

    [Header("Time")]
    [SerializeField] private float delaySpawnPiece;

    //private BoxCollider2D box2D; need ?
    private Animator anim;
    private bool isGotHit = false;
    private bool allowSpawnPiece = false;
    private bool hasSpawnPiece = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationController();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !isGotHit)
        {
            var rbPlayer = collision.gameObject.GetComponent<PlayerStateManager>();
            rbPlayer.GetRigidBody2D().AddForce(new Vector2(0f, forceApply));
            isGotHit = true; //Mark this box has been hitted and make sure only applied force once
            Invoke("AllowSpawnPiece", delaySpawnPiece);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            var bulletCtrl = collision.gameObject.GetComponent<BulletController>();
            bulletCtrl.SpawnBulletPieces();
            Destroy(bulletCtrl.gameObject);
            isGotHit = true; //Mark this box has been hitted and make sure only applied force once
            Invoke("AllowSpawnPiece", delaySpawnPiece);
        }
    }

    private void AnimationController()
    {
        if (isGotHit)
        {
            brokeSound.Play();
            anim.SetTrigger("GotHit");
            isGotHit = false;
        }
        if (allowSpawnPiece && !hasSpawnPiece)
        {
            SpawnPiece();
            SpawnGift();
            Destroy(this.gameObject);
        }
    }

    private void AllowSpawnPiece()
    {
        allowSpawnPiece = true;
    }

    private void SpawnPiece()
    {
        hasSpawnPiece = true;

        Instantiate(brPiece1, new Vector3(transform.position.x - .35f, transform.position.y + .33f, transform.position.z), Quaternion.identity, null);
        Instantiate(brPiece2, new Vector3(transform.position.x + .25f, transform.position.y + .33f, transform.position.z), Quaternion.identity, null);
        Instantiate(brPiece3, new Vector3(transform.position.x - .35f, transform.position.y - .33f, transform.position.z), Quaternion.identity, null);
        Instantiate(brPiece4, new Vector3(transform.position.x + .25f, transform.position.y - .33f, transform.position.z), Quaternion.identity, null);
        //Chú ý tham số cuối của hàm này, pass null nếu 0 muốn piece nhận thằng box làm parent :D
    }

    private void SpawnGift()
    {
        int randomGift = Random.Range(1, 6);
        switch (randomGift)
        {
            case 1:
                Instantiate(apple, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, null);
                //Debug.Log("A");
                break;
            case 2:
                Instantiate(banana, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, null);
                //Debug.Log("B");
                break;
            case 3:
                Instantiate(cherry, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, null);
                //Debug.Log("C");
                break;
            case 4:
                Instantiate(orange, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, null);
                //Debug.Log("O");
                break;

            case 5:
                Instantiate(mushroom, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, null);
                //Debug.Log("M");
                break;
        }
    }

}
