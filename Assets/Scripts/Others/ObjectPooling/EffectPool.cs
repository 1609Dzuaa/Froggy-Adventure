using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    //Define a pool class that maintains a collection of reusable objects
    //Pools are useful to avoid the cost of allocation and deallocation
    //Ref: https://gameprogrammingpatterns.com/object-pool.html

    private static EffectPool _EffectPoolInstance;
    private Dictionary<string, List<GameObject>> _dictEffectPool = new Dictionary<string, List<GameObject>>();

    [Header("Dashable")]
    [SerializeField] private int _dashableEffCount;
    [SerializeField] private GameObject _dashableEffectPrefabs;

    [Header("Bee")]
    [SerializeField] private int _poolBeeEffectCount;
    [SerializeField] private GameObject _beeEffectPrefabs;

    [Header("Trunk")]
    [SerializeField] private int _poolTrunkEffectCount;
    [SerializeField] private GameObject _trunkEffectPrefabs;

    public static EffectPool Instance
    {
        get
        {
            if (!_EffectPoolInstance)
            {
                _EffectPoolInstance = FindObjectOfType<EffectPool>();

                if (!_EffectPoolInstance)
                    Debug.Log("No EffectPool in scene");
            }
            return _EffectPoolInstance;
        }
    }

    private void Awake()
    {
        CreateInstance();
        InitDictionary();
    }

    private void InitDictionary()
    {
        _dictEffectPool.Add(GameConstants.DASHABLE_EFFECT, new List<GameObject>());
        //_dictEffectPool.Add(GameConstants.BEE_Effect, new List<GameObject>());
        //_dictEffectPool.Add(GameConstants.TRUNK_Effect, new List<GameObject>());
    }

    private void CreateInstance()
    {
        if (!_EffectPoolInstance)
        {
            _EffectPoolInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Add Effect vào pool và đánh dấu chưa active nó
        for (int i = 0; i < _dashableEffCount; i++)
            InstantiateEffect(_dashableEffectPrefabs, GameConstants.DASHABLE_EFFECT);

        /*for (int i = 0; i < _poolBeeEffectCount; i++)
            InstantiateEffect(_beeEffectPrefabs, GameConstants.BEE_Effect);

        for (int i = 0; i < _poolTrunkEffectCount; i++)
            InstantiateEffect(_trunkEffectPrefabs, GameConstants.TRUNK_Effect);*/
    }

    private void InstantiateEffect(GameObject gameObject, string EffectType)
    {
        GameObject gObj = Instantiate(gameObject);
        gObj.SetActive(false);
        _dictEffectPool[EffectType].Add(gObj);
    }

    public GameObject GetPoolObject(string EffectType)
    {
        for (int i = 0; i < _dictEffectPool[EffectType].Count; i++)
        {
            //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
            if (!_dictEffectPool[EffectType][i].activeInHierarchy)
            {
                Debug.Log("Effect: " + _dictEffectPool[EffectType][i].name + " " + i);
                return _dictEffectPool[EffectType][i];
            }
        }

        Debug.Log("out of effect");
        return null;
    }
}
