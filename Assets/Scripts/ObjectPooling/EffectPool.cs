using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    //Có vấn đề với bể Effect
    //Khi Load lại scene thì các Effect biến mất
    //Done, áp dụng tương tự cho các bể khác (Bullet, Pieces,...)

    private static EffectPool _EffectPoolInstance;
    private Dictionary<GameEnums.EEfects, List<GameObject>> _dictEffectPool = new();

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
    }

    private void CreateInstance()
    {
        if (!_EffectPoolInstance)
        {
            _EffectPoolInstance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("Khoi tao 1st");
        }
        else
            Destroy(gameObject);
    }

    private void InstantiateEffect(GameObject gameObject, GameEnums.EEfects effectType)
    {
        GameObject gObj = Instantiate(gameObject);
        gObj.SetActive(false);
        _dictEffectPool[effectType].Add(gObj);
    }

    private void InstantiateManyEffect(GameObject gameObject, GameEnums.EEfects effectType, int effCount)
    {
        for (int i = 0; i < effCount; i++)
        {
            GameObject gObj = Instantiate(gameObject);
            gObj.SetActive(false);
            _dictEffectPool[effectType].Add(gObj);
        }
    }

    public GameObject GetObjectInPool(GameEnums.EEfects EffectType)
    {
        for (int i = 0; i < _dictEffectPool[EffectType].Count; i++)
        {
            Debug.Log("Eff hien tai: " + _dictEffectPool[EffectType]);
            //Thực hiện việc check null trước khi lôi nó ra
            //Vì lúc Reload Scene có thể GameObject đó bị null
            if (_dictEffectPool[EffectType][i])
            {
                //Tìm xem trong cái pool có thằng nào 0 kích hoạt kh thì lôi nó ra
                if (!_dictEffectPool[EffectType][i].activeInHierarchy)
                {
                    //Debug.Log("Effect: " + _dictEffectPool[EffectType][i].name + " " + i);
                    return _dictEffectPool[EffectType][i];
                }
            }
        }

        Debug.Log("out of effect");
        return null;
    }

    private void AddVFXToDictionary(GameEnums.EEfects vfxName) 
    {
        if (!_dictEffectPool.ContainsKey(vfxName))
            _dictEffectPool.Add(vfxName, new List<GameObject>());
    }

    public void AddVFXToPool(GameObject vfx, GameEnums.EEfects vfxName, int ammount)
    {
        AddVFXToDictionary(vfxName);

        if (ammount == 1)
            InstantiateEffect(vfx, vfxName);
        else
            InstantiateManyEffect(vfx, vfxName, ammount);
    }
}
