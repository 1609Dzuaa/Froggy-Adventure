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
        //Xài cách này tha hồ điều chỉnh ngoài Inspector, hạn chế hard - coded

        //Mỗi lần bắn, tạo 1 viên đạn mới và set hướng cho nó
        //= chính hướng mặt hiện tại của cây(để điều chỉnh vector vận tốc)
        //Cách cũ: Instantiate(bullet, shootPosition.position, transform.rotation);
        //Là mình chỉ tạo bản sao của cái bullet(lúc này isDirectionRight của nó 
        //mặc định là false) dẫn đến việc vector vận tốc của bullet hđ 0 như ý
        //Cân nhắc xài Object Pool pattern
        GameObject bullet;
        bullet = Instantiate(_bullet, _shootPosition.position, transform.rotation);
        bullet.GetComponent<BulletController>().SetIsDirectionRight(_isFacingRight);
    }

}
