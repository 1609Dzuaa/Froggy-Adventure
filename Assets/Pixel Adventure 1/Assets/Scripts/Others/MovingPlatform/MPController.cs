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
        rb.velocity = new Vector2(-vX, rb.velocity.y);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.position.x < maxPointLeft.position.x)
            rb.velocity = new Vector2(vX, rb.velocity.y);
        else if (rb.position.x > maxPointRight.position.x)
            rb.velocity = new Vector2(-vX, rb.velocity.y);

        //Tư duy cũ => Kh thích hợp vì:
        //Khi move cái platform đó sang chỗ khác xa hơn
        //thì sẽ 0 đc như ý muốn
        /*if (rb.position.x < 15.0f)
            rb.velocity = new Vector2(vX, rb.velocity.y);
        else if (rb.position.x > 27.0f)
            rb.velocity = new Vector2(-vX, rb.velocity.y);*/
    }
}
