using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float vX;
    //2 cái vật thể dưới là mốc trái, phải của Platform -> ta sẽ 0 vẽ nó
    [SerializeField] Transform maxPointLeft;
    [SerializeField] Transform maxPointRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Người chơi bị văng 1 đoạn dù ở trên plat
        //Khả năng là do velocity
        //=>thử biến đổi vị trí = cách 0 dùng velo
    }

    private void FixedUpdate()
    {
        //Dùng cách này để khi SetParent thằng Player
        //thì nó sẽ chịu đứng yên hoàn toàn trên cái platform đó
        //(Do 0 có velocity)
        if (transform.position.x <= maxPointLeft.position.x || transform.position.x >= maxPointRight.position.x)
            vX = -vX;

        this.transform.position += new Vector3(vX, 0, 0) * Time.deltaTime;

        //Old stuff: Don't do the sh!t below:')
        //if (rb.position.x < maxPointLeft.position.x)
        //rb.velocity = new Vector2(vX, rb.velocity.y);
        //else if (rb.position.x > maxPointRight.position.x)
        //rb.velocity = new Vector2(-vX, rb.velocity.y);

        //Tư duy cũ => Kh thích hợp vì:
        //Khi move cái platform đó sang chỗ khác xa hơn
        //thì sẽ 0 đc như ý muốn
        //Đại khái hạn chế dùng hằng số
        /*if (rb.position.x < 15.0f)
            rb.velocity = new Vector2(vX, rb.velocity.y);
        else if (rb.position.x > 27.0f)
            rb.velocity = new Vector2(-vX, rb.velocity.y);*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Player")
            collision.collider.gameObject.transform.SetParent(this.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Player")
            collision.collider.gameObject.transform.SetParent(null);
    }
}
