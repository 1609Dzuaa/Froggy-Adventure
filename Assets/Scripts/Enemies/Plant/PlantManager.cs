using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class PlantManager : NMEnemiesManager
{
    [Header("Bullet & Shoot Pos")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPosition;

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

    //Event Func in Attack Animation
    private void SpawnBullet()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
            return;

        GameObject bullet = BulletPool.Instance.GetObjectInPool(GameConstants.PLANT_BULLET);

        bullet.SetActive(true);
        bullet.transform.position = _shootPosition.position;
        bullet.GetComponent<BulletController>().IsDirectionRight = _isFacingRight;
        bullet.GetComponent<BulletController>().Type = GameConstants.PLANT_BULLET;
        //Debug.Log("I'm here");
    }

}
