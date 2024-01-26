using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    private static BulletPool _bulletPoolInstance;
    private Dictionary<GameEnums.EEnemiesBullet, List<GameObject>> _dictBulletPool = new();

    [Header("Ammount")]
    [SerializeField] private int _bulletAmmount;

    public static BulletPool Instance 
    {
        get
        {
            if (!_bulletPoolInstance)
            {
                _bulletPoolInstance = FindObjectOfType<BulletPool>();

                if (!_bulletPoolInstance)
                    Debug.Log("No BulletPool in scene");
            }
            return _bulletPoolInstance;
        }
    }

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (!_bulletPoolInstance)
        {
            _bulletPoolInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void InstantiateBullet(GameObject gameObject, GameEnums.EEnemiesBullet bulletType, string bulletID)
    {
        for (int i = 0; i < _bulletAmmount; i++)
        {
            GameObject gObj = Instantiate(gameObject);
            gObj.SetActive(false);
            _dictBulletPool[bulletType].Add(gObj);
        }
    }

    public GameObject GetObjectInPool(GameEnums.EEnemiesBullet bulletType)
    {
        for (int i = 0; i < _dictBulletPool[bulletType].Count; i++)
        {
            //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
            if (!_dictBulletPool[bulletType][i].activeInHierarchy)
            {
                //Debug.Log("Bullet: " + _dictBulletPool[bulletType][i].name + " " + i);
                return _dictBulletPool[bulletType][i];
            }
        }

        Debug.Log("out of ammo");
        return null;
    }

    private void AddBulletToDictionary(GameEnums.EEnemiesBullet bulletType)
    {
        if (!_dictBulletPool.ContainsKey(bulletType))
            _dictBulletPool.Add(bulletType, new List<GameObject>());
    }

    public void AddBulletToPool(GameEnums.EEnemiesBullet bulletType, GameObject bullet, string bulletID)
    {
        AddBulletToDictionary(bulletType);
    }

}
