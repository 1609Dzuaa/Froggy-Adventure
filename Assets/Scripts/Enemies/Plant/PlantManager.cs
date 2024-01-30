using System.Collections;
using System.Collections.Generic;
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
        //prob here 
        //enemy's bug khi player vô hình lúc chbi attack khiến cho
        //dù hết buff vô hình và bị detect nhưng enemy vẫn 0 attack player
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
        {
            ChangeState(_nmEnemiesIdleState);
            return;
        }

        GameObject bullet = BulletPool.Instance.GetObjectInPool(GameEnums.EEnemiesBullet.Plant);
        bullet.SetActive(true);
        string bulletID = bullet.GetComponent<BulletController>().BulletID;

        BulletInfor info = new BulletInfor(GameEnums.EEnemiesBullet.Plant, bulletID, _isFacingRight, _shootPosition.position);
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.BulletOnReceiveInfo, info);
        //SoundsManager.Instance.GetTypeOfSound(GameConstants.PLANT_SHOOT_SOUND).Play();
        //Debug.Log("I'm here");
    }

}
