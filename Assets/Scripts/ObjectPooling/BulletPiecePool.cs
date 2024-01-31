using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class BulletPiecePool : BaseSingleton<BulletPiecePool>
{
    private Dictionary<EEnemiesBullet, List<BulletPiecePair>> _dictBulletPiecePiecePool = new();

    [Header("Ammount")]
    [SerializeField] int _piecesAmmount;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddBulletToPoolDictionary(EEnemiesBullet bulletType)
    {
        if (!_dictBulletPiecePiecePool.ContainsKey(bulletType))
            _dictBulletPiecePiecePool.Add(bulletType, new List<BulletPiecePair>());
    }

    public void InstantiateBulletPiece(BulletPiecePair gameObjectPair, EEnemiesBullet BulletPieceType)
    {
        GameObject gObj1 = Instantiate(gameObjectPair.Pair1);
        gObj1.SetActive(false);
        GameObject gObj2 = Instantiate(gameObjectPair.Pair2);
        gObj2.SetActive(false);
        BulletPiecePair piecePair = new(gObj1, gObj2);
        _dictBulletPiecePiecePool[BulletPieceType].Add(piecePair);
    }

    private void AddPiecesPairToList(EEnemiesBullet BulletPieceType, BulletPiecePair piecePair )
    {
        _dictBulletPiecePiecePool[BulletPieceType].Add(piecePair);
    }

    public BulletPiecePair GetObjectInPool(EEnemiesBullet bulletType)
    {
        BulletPiecePair bulletPiecePair = new();

        for (int i = 0; i < _dictBulletPiecePiecePool[bulletType].Count; i++)
        {
            if(_dictBulletPiecePiecePool[bulletType][i].Pair1 && _dictBulletPiecePiecePool[bulletType][i].Pair2)
            {
                if (!_dictBulletPiecePiecePool[bulletType][i].Pair1.activeInHierarchy && !_dictBulletPiecePiecePool[bulletType][i].Pair2.activeInHierarchy)
                {
                    //Debug.Log("BulletPiece: " + _dictBulletPiecePiecePool[BulletPieceType][i].Pair1.name + " " + i);
                    return _dictBulletPiecePiecePool[bulletType][i];
                }
            }
        }

        Debug.Log("out of piece");
        return bulletPiecePair;
    }
}
