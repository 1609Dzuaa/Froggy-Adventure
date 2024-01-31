using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameEnums;

public class BulletPool : BaseSingleton<BulletPool>
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    private Dictionary<EEnemiesBullet, List<GameObject>> _dictBulletPool = new();

    [Header("Ammount")]
    [SerializeField] private int _bulletAmmount;

    [SerializeField] GameObject _plant;

    public Dictionary<EEnemiesBullet, List<GameObject>> GetDictBulletPool() { return _dictBulletPool; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        AddBulletToDictionary(EEnemiesBullet.Plant);
        InstantiateBullets(_plant, EEnemiesBullet.Plant, _bulletAmmount);
    }

    private void AddBulletToDictionary(EEnemiesBullet bulletName)
    {
        if (!_dictBulletPool.ContainsKey(bulletName))
            _dictBulletPool.Add(bulletName, new List<GameObject>());
    }

    /*public void AddBulletToPool(GameObject bullet, EEnemiesBullet bulletName, int ammount)
    {
        AddBulletToDictionary(bulletName);
        InstantiateBullets(bullet, bulletName, ammount);
    }*/

    private void InstantiateBullets(GameObject gameObject, EEnemiesBullet bulletName, int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject gObj = Instantiate(gameObject);
            gObj.SetActive(false);
            _dictBulletPool[bulletName].Add(gObj);
        }
    }

    public GameObject GetObjectInPool(EEnemiesBullet bulletType)
    {
        for (int i = 0; i < _dictBulletPool[bulletType].Count; i++)
        {
            if (_dictBulletPool[bulletType][i])
            {
                //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
                if (!_dictBulletPool[EEnemiesBullet.Plant][i].activeInHierarchy)
                {
                    //Debug.Log("Bullet: " + _dictBulletPool[bulletType][i].name + " " + i);
                    return _dictBulletPool[EEnemiesBullet.Plant][i];
                }
            }
        }

        Debug.Log("out of ammo");
        return null;
    }

}