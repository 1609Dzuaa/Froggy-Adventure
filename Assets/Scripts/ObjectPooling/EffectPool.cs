using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class EffectPool : BaseSingleton<EffectPool>
{
    private Dictionary<EEfects, List<GameObject>> _dictEffectPool = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void AddVFXToDictionary(EEfects vfxName)
    {
        if (!_dictEffectPool.ContainsKey(vfxName))
            _dictEffectPool.Add(vfxName, new List<GameObject>());
    }

    public void AddVFXToPool(GameObject vfx, EEfects vfxName, int ammount)
    {
        AddVFXToDictionary(vfxName);
        InstantiateManyEffect(vfx, vfxName, ammount);
    }

    private void InstantiateManyEffect(GameObject gameObject, EEfects effectType, int effCount)
    {
        for (int i = 0; i < effCount; i++)
        {
            GameObject gObj = Instantiate(gameObject);
            gObj.SetActive(false);
            _dictEffectPool[effectType].Add(gObj);
        }
    }

    public GameObject GetObjectInPool(EEfects EffectType)
    {
        for (int i = 0; i < _dictEffectPool[EffectType].Count; i++)
        {
            //Debug.Log("Eff hien tai: " + _dictEffectPool[EffectType]);
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
}
