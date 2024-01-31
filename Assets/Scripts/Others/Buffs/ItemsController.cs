using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    [SerializeField] protected GameEnums.EBuffs _buff;

    private void Start() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.CollectFruitSfx, 1.0f);
            BuffsManager.Instance.GetTypeOfBuff(_buff).ApplyBuff();
            SpawnEffect();
            Destroy(gameObject);
        }
    }

    protected virtual void SpawnEffect()
    {
        GameObject collectEff = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.CollectFruits);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
