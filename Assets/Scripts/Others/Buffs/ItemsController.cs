using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    //Cho custom loại buff, đéo phải hard-code như trước
    [SerializeField] protected GameEnums.EBuffs _buff;

    private void Start()
    {
        //Register func, 0 GỌI, ApplyBuff đc gọi <=> Collide Trigger tag "Buff" trong PlayerManager
        PlayerStateManager.OnAppliedBuff += ApplyBuff;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameConstants.PLAYER_NAME)
        {
            SoundsManager.Instance.GetTypeOfSound(GameConstants.COLLECT_FRUITS_SOUND).Play();
            SpawnEffect();
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        //huỷ đăng ký
        PlayerStateManager.OnAppliedBuff -= ApplyBuff;
        Debug.Log("Da huy dky event");
    }

    protected virtual void ApplyBuff()
    {
        Debug.Log("0 co gi");
        //Each item will apply different buff in here
    }

    protected virtual void SpawnEffect()
    {
        GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameConstants.COLLECT_FRUITS_EFFECT);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
