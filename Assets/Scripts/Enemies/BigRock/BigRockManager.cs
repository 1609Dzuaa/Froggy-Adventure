using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRockManager : MEnemiesManager
{
    //Con vk này có 3 form: Big - Medium - Tiny
    //Từ Big về các form dưới sẽ tăng số lượng theo cấp số nhân của 2
    //Add effect cho nó sau
    //Add effect = particle system ?
    [Tooltip("0: Big|1: Medium|2: Tiny")]
    [Header("Type and Rock Clone"), Range(0, 2)]
    [SerializeField] private int _type;
    [SerializeField] private GameObject _rockClone;

    [Header("SpawnPos")]
    [SerializeField] private Transform _spawnPos1;
    [SerializeField] private Transform _spawnPos2;

    protected override void Awake()
    {
        base.Awake(); 
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            FlippingSprite();
    }

    protected virtual void SpawnClone()
    {
        switch(_type)
        {
            case 0:
                GameObject medRock1 = Instantiate(_rockClone, _spawnPos1.transform.position, Quaternion.identity, null);
                GameObject medRock2 = Instantiate(_rockClone, _spawnPos2.transform.position, Quaternion.identity, null);
                //var rock1Script = medRock1.GetComponentInChildren<BigRockManager>();
                //rock1Script.GetRigidbody2D().AddForce(new Vector2(-3f, 3f), ForceMode2D.Impulse);
                //var rock2Script = medRock2.GetComponentInChildren<BigRockManager>();
                //rock2Script.GetRigidbody2D().AddForce(new Vector2(3f, 3f), ForceMode2D.Impulse);
                break;
            case 1:
                /*GameObject tinyRock1 = Instantiate(_rockClone, _spawnPos.transform.position, Quaternion.identity, null);
                GameObject tinyRock2 = Instantiate(_rockClone, _spawnPos.transform.position, Quaternion.identity, null);
                GameObject tinyRock3 = Instantiate(_rockClone, _spawnPos.transform.position, Quaternion.identity, null);
                GameObject tinyRock4 = Instantiate(_rockClone, _spawnPos.transform.position, Quaternion.identity, null);*/
                break;
        }
        //Destroy(this.gameObject);
    }
}
