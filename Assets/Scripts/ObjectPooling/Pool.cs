using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class Pool : BaseSingleton<Pool>
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    private Dictionary<EPoolable, List<GameObject>> _dictPool = new();
    private Dictionary<EPoolable, List<BulletPiecePair>> _dictBPPPool = new();

    #region Vfxs
    [SerializeField] GameObject _dashableVfx;
    [SerializeField] GameObject _hitShieldVfx;
    [SerializeField] GameObject _geckoAppearVfx;
    [SerializeField] GameObject _geckoDisappearVfx;
    [SerializeField] GameObject _collectFruitsVfx;
    [SerializeField] GameObject _collectHPVfx;
    [SerializeField] GameObject _brownExplosionVfx;
    [SerializeField] int _vfxAmmount;
    #endregion

    #region Bullets
    [SerializeField] GameObject _plantBullet;
    [SerializeField] GameObject _beeBullet;
    [SerializeField] GameObject _trunkBullet;
    [SerializeField] int _bulletAmmount;
    #endregion

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

        InstantiateGameObjects(_dashableVfx, EPoolable.Dashable, _vfxAmmount);
        InstantiateGameObjects(_hitShieldVfx, EPoolable.HitShield, _vfxAmmount);
        InstantiateGameObjects(_geckoAppearVfx, EPoolable.GeckoAppear, _vfxAmmount);
        InstantiateGameObjects(_geckoDisappearVfx, EPoolable.GeckoDisappear, _vfxAmmount);
        InstantiateGameObjects(_collectFruitsVfx, EPoolable.CollectFruits, _vfxAmmount);
        InstantiateGameObjects(_collectHPVfx, EPoolable.CollectHP, _vfxAmmount);
        InstantiateGameObjects(_brownExplosionVfx, EPoolable.BrownExplosion, _vfxAmmount);

        InstantiateGameObjects(_plantBullet, EPoolable.PlantBullet, _bulletAmmount);
        InstantiateGameObjects(_beeBullet, EPoolable.BeeBullet, _bulletAmmount);
        InstantiateGameObjects(_trunkBullet, EPoolable.TrunkBullet, _bulletAmmount);

        InstantiateBulletPiece(_plantBulletPiece1, _plantBulletPiece2, EPoolable.PlantBullet, _piecePairAmmount);
        InstantiateBulletPiece(_beeBulletPiece1, _beeBulletPiece2, EPoolable.BeeBullet, _piecePairAmmount);
        InstantiateBulletPiece(_trunkBulletPiece1, _trunkBulletPiece2, EPoolable.TrunkBullet, _piecePairAmmount);
    }

    private void FillInFirstDictionary()
    {
        foreach(EPoolable e in Enum.GetValues(typeof(EPoolable)))
            if (!_dictPool.ContainsKey(e))
                _dictPool.Add(e, new List<GameObject>());
    }

    private void FillInSecondDictionary()
    {
        _dictBPPPool.Add(EPoolable.PlantBullet, new());
        _dictBPPPool.Add(EPoolable.BeeBullet, new());
        _dictBPPPool.Add(EPoolable.TrunkBullet, new());
    }

    private void InstantiateGameObjects(GameObject gameObject, EPoolable bulletName, int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            GameObject gObj = Instantiate(gameObject);
            gObj.SetActive(false);
            _dictPool[bulletName].Add(gObj);
        }
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

    public GameObject GetObjectInPool(EPoolable bulletType)
    {
        for (int i = 0; i < _dictPool[bulletType].Count; i++)
        {
            //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
            if (!_dictPool[bulletType][i].activeInHierarchy)
            {
                //Debug.Log("Bullet: " + _dictPool[bulletType][i].name + " " + i);
                return _dictPool[bulletType][i];
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