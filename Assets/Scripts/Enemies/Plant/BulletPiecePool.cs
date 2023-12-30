using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletPiecePair
{
    private GameObject _pair1; 
    private GameObject _pair2;

    public BulletPiecePair(GameObject pair1, GameObject pair2)
    {
        _pair1 = pair1;
        _pair2 = pair2;
    }

    public GameObject Pair1 { get { return _pair1; } }

    public GameObject Pair2 { get { return _pair2; } }
}

public class BulletPiecePool : MonoBehaviour
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    private static BulletPiecePool _BulletPiecePiecePoolInstance;
    private Dictionary<int, List<BulletPiecePair>> _dictBulletPiecePiecePool = new Dictionary<int, List<BulletPiecePair>>();

    [Header("Plant")]
    [SerializeField] private int _poolPlantBulletPieceCount;
    [SerializeField] private GameObject _plantBulletPiecePrefabs1;
    [SerializeField] private GameObject _plantBulletPiecePrefabs2;

    [Header("Bee")]
    [SerializeField] private int _poolBeeBulletPieceCount;
    [SerializeField] private GameObject _beeBulletPiecePrefabs1;
    [SerializeField] private GameObject _beeBulletPiecePrefabs2;

    [Header("Trunk")]
    [SerializeField] private int _poolTrunkBulletPieceCount;
    [SerializeField] private GameObject _trunkBulletPiecePrefabs1;
    [SerializeField] private GameObject _trunkBulletPiecePrefabs2;

    public static BulletPiecePool Instance
    {
        get
        {
            if (!_BulletPiecePiecePoolInstance)
            {
                _BulletPiecePiecePoolInstance = FindObjectOfType<BulletPiecePool>();

                if (!_BulletPiecePiecePoolInstance)
                    Debug.Log("No BulletPiecePool in scene");
            }
            return _BulletPiecePiecePoolInstance;
        }
    }
    private void Awake()
    {
        CreateInstance();
        InitDictionary();
    }

    private void InitDictionary()
    {
        _dictBulletPiecePiecePool.Add(GameConstants.PLANT_BULLET, new List<BulletPiecePair>());
        _dictBulletPiecePiecePool.Add(GameConstants.BEE_BULLET, new List<BulletPiecePair>());
        _dictBulletPiecePiecePool.Add(GameConstants.TRUNK_BULLET, new List<BulletPiecePair>());
    }

    private void CreateInstance()
    {
        if (!_BulletPiecePiecePoolInstance)
        {
            _BulletPiecePiecePoolInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //Add BulletPiece vào pool và đánh dấu chưa active nó
        for (int i = 0; i < _poolPlantBulletPieceCount; i++)
            InstantiateBulletPiece(new BulletPiecePair(_plantBulletPiecePrefabs1, _plantBulletPiecePrefabs2), GameConstants.PLANT_BULLET);

        for (int i = 0; i < _poolBeeBulletPieceCount; i++)
            InstantiateBulletPiece(new BulletPiecePair(_beeBulletPiecePrefabs1, _beeBulletPiecePrefabs2), GameConstants.BEE_BULLET);

        for (int i = 0; i < _poolTrunkBulletPieceCount; i++)
            InstantiateBulletPiece(new BulletPiecePair(_trunkBulletPiecePrefabs1, _trunkBulletPiecePrefabs2), GameConstants.TRUNK_BULLET);
    }

    private void InstantiateBulletPiece(BulletPiecePair gameObjectPair, int BulletPieceType)
    {
        GameObject gObj1 = Instantiate(gameObjectPair.Pair1);
        gObj1.SetActive(false);
        GameObject gObj2 = Instantiate(gameObjectPair.Pair2);
        gObj2.SetActive(false);
        BulletPiecePair piecePair = new BulletPiecePair(gObj1, gObj2);
        _dictBulletPiecePiecePool[BulletPieceType].Add(piecePair);
    }

    public BulletPiecePair GetPoolObject(int BulletPieceType)
    {
        BulletPiecePair bulletPiecePair = new BulletPiecePair();

        for (int i = 0; i < _dictBulletPiecePiecePool[BulletPieceType].Count; i++)
        {
            //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
            if (!_dictBulletPiecePiecePool[BulletPieceType][i].Pair1.activeInHierarchy && !_dictBulletPiecePiecePool[BulletPieceType][i].Pair2.activeInHierarchy)
            {
                Debug.Log("BulletPiece: " + _dictBulletPiecePiecePool[BulletPieceType][i].Pair1.name + " " + i);
                return _dictBulletPiecePiecePool[BulletPieceType][i];
            }
        }

        //Debug.Log("out of ammo");
        return bulletPiecePair;
    }
}
