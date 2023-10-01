using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPController : MonoBehaviour
{
    [SerializeField] float vX;
    //2 cái vật thể dưới là mốc trái, phải của Platform -> ta sẽ 0 vẽ nó
    [SerializeField] Transform maxPointLeft;
    [SerializeField] Transform maxPointRight;
    Rigidbody2D rb;
    private void Start()
    {
        
    }

    void Update()
    {
        //Còn chút giật khi Player di chuyển khỏi platform
        //=>Khả năng cũng do velocity
        //Debug.Log(rb.velocity.x);
        //Thử đặt trigger cho box collider onTop
    }

    private void FixedUpdate()
    {
        //Dùng cách này để khi SetParent thằng Player
        //thì nó sẽ chịu đứng yên hoàn toàn trên cái platform đó
        //(Do 0 có velocity)
        if (transform.position.x <= maxPointLeft.position.x || transform.position.x >= maxPointRight.position.x)
            vX = -vX;

        this.transform.position += new Vector3(vX, 0, 0) * Time.deltaTime;
    }
}
