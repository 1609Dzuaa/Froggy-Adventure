using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkManager : MEnemiesManager
{
    //Nghiên cứu object pool tối ưu cho các enemy spawn đạn

    [Header("Weapon Field")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPos;

    private TrunkAttackState _trunkAttackState = new();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        MEnemiesAttackState = _trunkAttackState;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void SpawnBullet()
    {
        if (PlayerShieldBuff.Instance.IsAllowToUpdate)
            return;

        GameObject bullet;
        bullet = Instantiate(_bullet, _shootPos.position, transform.rotation);
        bullet.GetComponent<BulletController>().SetIsDirectionRight(_isFacingRight);
        //Event của animation Attack
    }
}
