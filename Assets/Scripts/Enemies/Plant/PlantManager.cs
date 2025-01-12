using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : NMEnemiesManager
{
    [Header("Bullet & Shoot Pos")]
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
        if (BuffsManager.Instance.GetBuff(GameEnums.EBuffs.Invisible).IsActivating)
        {
            ChangeState(_nmEnemiesIdleState);
            return;
        }

        GameObject bullet = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.PlantBullet);
        string bulletID = "";
        bullet.SetActive(true);
        bulletID = bullet.GetComponent<BulletController>().BulletID;

        BulletInfor info = new BulletInfor(GameEnums.EPoolable.PlantBullet, bulletID, _isFacingRight, _shootPosition.position);
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.BulletOnReceiveInfo, info);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.PlantShootSfx, 1.0f);
        //Debug.Log("ID!: " + bulletID);
    }

}
