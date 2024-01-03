using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    private static EffectPool _EffectPoolInstance;
    private Dictionary<string, List<GameObject>> _dictEffectPool = new Dictionary<string, List<GameObject>>();

    [Header("Dashable")]
    [SerializeField] private GameObject _dashableEffect;

    [Header("Gecko")]
    [SerializeField] private GameObject _geckoAppearEffect;
    [SerializeField] private GameObject _geckoDisappearEffect;

    [Header("Hit Shield")]
    [SerializeField] private int _hitShieldEffectCount; //nhiều HitShieldEff tí vì có thể bị bắn nhiều hướng
    [SerializeField] private GameObject _hitShieldEffect;

    [Header("CollectHP")]
    [SerializeField] private GameObject _collectHPEffect;

    [Header("Brown Explosion")]
    [SerializeField] private int _brownExplosionEffectCount;
    [SerializeField] private GameObject _brownExplosionEffect;

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
        _dictEffectPool.Add(GameConstants.GECKO_APPEAR_EFFECT, new List<GameObject>());
        _dictEffectPool.Add(GameConstants.GECKO_DISAPPEAR_EFFECT, new List<GameObject>());
        _dictEffectPool.Add(GameConstants.HIT_SHIELD_EFFECT, new List<GameObject>());
        _dictEffectPool.Add(GameConstants.COLLECT_HP_EFFECT, new List<GameObject>());
        _dictEffectPool.Add(GameConstants.BROWN_EXPLOSION, new List<GameObject>());
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
        AddEffectToPool();
    }

    private void AddEffectToPool()
    {
        //Add Effect vào pool và đánh dấu chưa active nó
        InstantiateEffect(_dashableEffect, GameConstants.DASHABLE_EFFECT);
        InstantiateEffect(_geckoAppearEffect, GameConstants.GECKO_APPEAR_EFFECT);
        InstantiateEffect(_geckoDisappearEffect, GameConstants.GECKO_DISAPPEAR_EFFECT);
        InstantiateManyEffect(_hitShieldEffect, GameConstants.HIT_SHIELD_EFFECT, _hitShieldEffectCount);
        InstantiateEffect(_collectHPEffect, GameConstants.COLLECT_HP_EFFECT);
        InstantiateManyEffect(_brownExplosionEffect, GameConstants.BROWN_EXPLOSION, _brownExplosionEffectCount);
    }

    private void InstantiateEffect(GameObject gameObject, string effectType)
    {
        GameObject gObj = Instantiate(gameObject);
        gObj.SetActive(false);
        _dictEffectPool[effectType].Add(gObj);
    }

    private void InstantiateManyEffect(GameObject gameObject, string effectType, int effCount)
    {
        for (int i = 0; i < effCount; i++)
        {
            GameObject gObj = Instantiate(gameObject);
            gObj.SetActive(false);
            _dictEffectPool[effectType].Add(gObj);
        }
    }

    public GameObject GetObjectInPool(string EffectType)
    {
        for (int i = 0; i < _dictEffectPool[EffectType].Count; i++)
        {
            //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
            if (!_dictEffectPool[EffectType][i].activeInHierarchy)
            {
                //Debug.Log("Effect: " + _dictEffectPool[EffectType][i].name + " " + i);
                return _dictEffectPool[EffectType][i];
            }
        }

        Debug.Log("out of effect");
        return null;
    }
}
