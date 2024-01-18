using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    private static BulletPool _bulletPoolInstance;
    private Dictionary<string, List<GameObject>> _dictBulletPool = new Dictionary<string, List<GameObject>>();

    [Header("Plant")]
    [SerializeField] private int _poolPlantBulletCount;
    [SerializeField] private GameObject _plantBulletPrefabs;

    [Header("Bee")]
    [SerializeField] private int _poolBeeBulletCount;
    [SerializeField] private GameObject _beeBulletPrefabs;

    [Header("Trunk")]
    [SerializeField] private int _poolTrunkBulletCount;
    [SerializeField] private GameObject _trunkBulletPrefabs;

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
        InitDictionary();
    }

    private void InitDictionary()
    {
        _dictBulletPool.Add(GameConstants.PLANT_BULLET, new List<GameObject>());
        _dictBulletPool.Add(GameConstants.BEE_BULLET, new List<GameObject>());
        _dictBulletPool.Add(GameConstants.TRUNK_BULLET, new List<GameObject>());
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

    // Start is called before the first frame update
    void Start()
    {
        AddBulletsToPool();
    }

    private void AddBulletsToPool()
    {
        //Add bullet vào pool và đánh dấu chưa active nó
        for (int i = 0; i < _poolPlantBulletCount; i++)
        {
            string bulletID = GameConstants.PLANT_BULLET + i.ToString();
            InstantiateBullet(_plantBulletPrefabs, GameConstants.PLANT_BULLET, bulletID);
        }

        for (int i = 0; i < _poolBeeBulletCount; i++)
        {
            string bulletID = GameConstants.BEE_BULLET + i.ToString();
            InstantiateBullet(_beeBulletPrefabs, GameConstants.BEE_BULLET, bulletID);
        }

        for (int i = 0; i < _poolTrunkBulletCount; i++)
        {
            string bulletID = GameConstants.TRUNK_BULLET + i.ToString();
            InstantiateBullet(_trunkBulletPrefabs, GameConstants.TRUNK_BULLET, bulletID);
        }
    }

    private void InstantiateBullet(GameObject gameObject, string bulletType, string bulletID)
    {
        GameObject gObj = Instantiate(gameObject);
        gObj.SetActive(false);
        _dictBulletPool[bulletType].Add(gObj);
        gObj.GetComponent<BulletController>().BulletID = bulletID;
    }

    public GameObject GetObjectInPool(string bulletType)
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
}
