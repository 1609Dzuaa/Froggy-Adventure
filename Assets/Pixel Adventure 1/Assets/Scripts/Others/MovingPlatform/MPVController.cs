using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPVController : MonoBehaviour
{
    [SerializeField] float vY;
    //2 cái vật thể dưới là mốc trái, phải của Platform -> ta sẽ 0 vẽ nó
    [SerializeField] Transform maxPointTop;
    [SerializeField] Transform maxPointBottom;
    Rigidbody2D rb;
    private void Start()
    {

    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {        
        if (transform.position.y >= maxPointTop.position.y || transform.position.y <= maxPointBottom.position.y)
            vY = -vY;

        this.transform.position += new Vector3(0, vY, 0) * Time.deltaTime;
    }
}