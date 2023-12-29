using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleRockManager : MEnemiesManager
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
    [SerializeField] private Transform _spawnPos3;
    [SerializeField] private Transform _spawnPos4;

    [Header("Effect")]
    [SerializeField] private GameObject _deadEffect;

    public void SetIsFacingRight(bool isFacingRight) { this._isFacingRight = isFacingRight; }

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

    protected virtual void SpawnClone()
    {
        switch(_type)
        {
            case 0:
                Instantiate(_deadEffect, transform.position, Quaternion.identity, null);
                GameObject medRock1 = Instantiate(_rockClone, _spawnPos1.transform.position, Quaternion.identity, null);
                GameObject medRock2 = Instantiate(_rockClone, _spawnPos2.transform.position, Quaternion.identity, null);
                /*medRock1.GetComponent<BigRockManager>().SetIsFacingRight(true);
                medRock2.GetComponent<BigRockManager>().SetIsFacingRight(false);*/
                break;
            case 1:
                Instantiate(_deadEffect, transform.position, Quaternion.identity, null);
                GameObject tinyRock1 = Instantiate(_rockClone, _spawnPos1.transform.position, Quaternion.identity, null);
                GameObject tinyRock2 = Instantiate(_rockClone, _spawnPos2.transform.position, Quaternion.identity, null);
                break;
        }
        Destroy(this.gameObject);
    }
}
