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

[System.Serializable]
public struct BulletPiecePair
{
    public EPoolable _bulletType;
    public GameObject _pair1;
    public GameObject _pair2;
    public int _ammount;

    public BulletPiecePair(EPoolable bulletType, GameObject pair1, GameObject pair2, int ammount)
    {
        _bulletType = bulletType;
        _pair1 = pair1;
        _pair2 = pair2;
        _ammount = ammount;
    }

    public GameObject Pair1 { get { return _pair1; } }

    public GameObject Pair2 { get { return _pair2; } }
}

public class Pool : BaseSingleton<Pool>
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    [SerializeField] List<PoolableObject> _listPoolableObj = new();
    [SerializeField] List<BulletPiecePair> _listBPiecePair = new();
    private Dictionary<EPoolable, List<GameObject>> _dictPool = new();
    private Dictionary<EPoolable, List<BulletPiecePair>> _dictBPPPool = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        FillInFirstDictionary();
        FillInSecondDictionary();

        InstantiateGameObjects();
    }

    private void FillInFirstDictionary()
    {
        for (int i = 0; i < _listPoolableObj.Count; i++)
            if (!_dictPool.ContainsKey(_listPoolableObj[i]._ePoolable))
                _dictPool.Add(_listPoolableObj[i]._ePoolable, new());
    }

    private void FillInSecondDictionary()
    {
        for (int i = 0; i < _listBPiecePair.Count; i++)
            if (!_dictBPPPool.ContainsKey(_listBPiecePair[i]._bulletType))
                _dictBPPPool.Add(_listBPiecePair[i]._bulletType, new());
    }

    private void InstantiateGameObjects()
    {
        //Duyệt này hơi chuối nhưng ch tìm đc cách khác
        for (int i = 0; i < _listPoolableObj.Count; i++)
        {
            for(int j = 0; j < _listPoolableObj[i]._ammount; j++)
            {
                //Awake & OnEnable của gObj vẫn đc gọi dù SetActive(false)
                GameObject gObj = Instantiate(_listPoolableObj[i]._GObjPoolable);
                gObj.SetActive(false);
                _dictPool[_listPoolableObj[i]._ePoolable].Add(gObj);
            }
        }

        for (int i = 0; i < _listBPiecePair.Count; i++)
        {
            for (int j = 0; j < _listBPiecePair[i]._ammount; j++)
            {
                //Awake & OnEnable của gObj vẫn đc gọi dù SetActive(false)
                GameObject gObj1 = Instantiate(_listBPiecePair[i]._pair1);
                gObj1.SetActive(false);
                GameObject gObj2 = Instantiate(_listBPiecePair[i]._pair2);
                gObj2.SetActive(false);
                BulletPiecePair bPP = new(_listBPiecePair[i]._bulletType, gObj1, gObj2, _listBPiecePair[i]._ammount);
                _dictBPPPool[_listBPiecePair[i]._bulletType].Add(bPP);
            }
        }
        //Duyệt từng object kiểu PoolableObjects trong cái list
        //Add GameObject của object đó dựa trên lượng của object đó
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

        Debug.Log("out of " + objType);
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

        Debug.Log("out of piece " + bulletType);
        return bulletPiecePair;
    }

}