using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPHController : MonoBehaviour
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
        //Solution:
        //1.Raycast ?
        //2.FootCheck
        //3.FF :)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Player")
            //collision.gameObject.transform.SetParent(this.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Player")
            //collision.gameObject.transform.SetParent(null);
    }
}
