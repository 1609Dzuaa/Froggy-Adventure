using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

[System.Serializable]
public struct PoolableObject
{
    public EPoolable _ePoolable;
    public GameObject _GObjPoolable;
    public int _ammount;
}

public class Pool : BaseSingleton<Pool>
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    [SerializeField] List<PoolableObject> _list = new();
    private Dictionary<EPoolable, List<GameObject>> _dictPool = new();
    private Dictionary<EPoolable, List<BulletPiecePair>> _dictBPPPool = new();

    #region BulletPieces
    [SerializeField] GameObject _plantBulletPiece1;
    [SerializeField] GameObject _plantBulletPiece2;
    [SerializeField] GameObject _beeBulletPiece1;
    [SerializeField] GameObject _beeBulletPiece2;
    [SerializeField] GameObject _trunkBulletPiece1;
    [SerializeField] GameObject _trunkBulletPiece2;
    [SerializeField] int _piecePairAmmount;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        FillInFirstDictionary();
        FillInSecondDictionary();

        InstantiateGameObjects();

        InstantiateBulletPiece(_plantBulletPiece1, _plantBulletPiece2, EPoolable.PlantBullet, _piecePairAmmount);
        InstantiateBulletPiece(_beeBulletPiece1, _beeBulletPiece2, EPoolable.BeeBullet, _piecePairAmmount);
        InstantiateBulletPiece(_trunkBulletPiece1, _trunkBulletPiece2, EPoolable.TrunkBullet, _piecePairAmmount);
    }

    private void FillInFirstDictionary()
    {
        for (int i = 0; i < _list.Count; i++)
            if (!_dictPool.ContainsKey(_list[i]._ePoolable))
                _dictPool.Add(_list[i]._ePoolable, new());
    }

    private void FillInSecondDictionary()
    {
        _dictBPPPool.Add(EPoolable.PlantBullet, new());
        _dictBPPPool.Add(EPoolable.BeeBullet, new());
        _dictBPPPool.Add(EPoolable.TrunkBullet, new());
    }

    private void InstantiateGameObjects()
    {
        //Duyệt này hơi chuối nhưng ch tìm đc cách khác
        for (int i = 0; i < _list.Count; i++)
        {
            for(int j = 0; j < _list[i]._ammount; j++)
            {
                //Awake & OnEnable của gObj vẫn đc gọi dù SetActive(false)
                GameObject gObj = Instantiate(_list[i]._GObjPoolable);
                gObj.SetActive(false);
                _dictPool[_list[i]._ePoolable].Add(gObj);
            }
        }
        //Duyệt từng object kiểu PoolableObjects trong cái list
        //Add GameObject của object đó dựa trên lượng của object đó
    }

    private void InstantiateBulletPiece(GameObject piecepair1, GameObject piecepair2, EPoolable BulletPieceType, int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            GameObject gObj1 = Instantiate(piecepair1);
            gObj1.SetActive(false);
            GameObject gObj2 = Instantiate(piecepair2);
            gObj2.SetActive(false);
            BulletPiecePair piecePair = new(gObj1, gObj2);
            _dictBPPPool[BulletPieceType].Add(piecePair);
        }
    }

    public GameObject GetObjectInPool(EPoolable objType)
    {
        for (int i = 0; i < _dictPool[objType].Count; i++)
        {
            //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
            if (!_dictPool[objType][i].activeInHierarchy)
            {
                //Debug.Log("Bullet: " + _dictPool[bulletType][i].name + " " + i);
                return _dictPool[objType][i];
            }
        }

        Debug.Log("out of ammo");
        return null;
    }

    public BulletPiecePair GetPiecePairInPool(EPoolable bulletType)
    {
        BulletPiecePair bulletPiecePair = new();

        for (int i = 0; i < _dictBPPPool[bulletType].Count; i++)
        {
            if (_dictBPPPool[bulletType][i].Pair1 && _dictBPPPool[bulletType][i].Pair2)
            {
                if (!_dictBPPPool[bulletType][i].Pair1.activeInHierarchy && !_dictBPPPool[bulletType][i].Pair2.activeInHierarchy)
                {
                    //Debug.Log("BulletPiece: " + _dictBPPPool[BulletPieceType][i].Pair1.name + " " + i);
                    return _dictBPPPool[bulletType][i];
                }
            }
        }

        Debug.Log("out of piece");
        return bulletPiecePair;
    }

}