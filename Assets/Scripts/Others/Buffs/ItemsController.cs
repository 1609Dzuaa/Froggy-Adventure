using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    //Cho custom loại buff, đéo phải hard-code như trước
    [SerializeField] protected GameEnums.EBuffs _buff;
    
    public GameEnums.EBuffs Buff { get => _buff; }

    private void Start()
    {

    }

    private void OnEnable()
    {
        //Register func, 0 GỌI, ApplyBuff đc gọi <=> Collide Trigger tag "Buff" trong PlayerManager
        //EventsManager.Instance.SubcribeAnEvent(GameEnums.EEvents.PlayerOnAbsorbBuffs, ApplyBuffToPlayer);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            SoundsManager.Instance.GetTypeOfSound(GameConstants.COLLECT_FRUITS_SOUND).Play();
            SpawnEffect();
            //Debug.Log("type: " + _buff);
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        //huỷ đăng ký
        EventsManager.Instance.UnsubcribeAnEvent(GameEnums.EEvents.PlayerOnAbsorbBuffs, ApplyBuffToPlayer);
        //Debug.Log("Da huy dky event");
    }

    protected virtual void ApplyBuffToPlayer(object obj)
    {
        if (_buff == (GameEnums.EBuffs)obj)
            BuffsManager.Instance.GetTypeOfBuff(_buff).ApplyBuff();
    }

    protected virtual void SpawnEffect()
    {
        GameObject collectEff = EffectPool.Instance.GetObjectInPool(GameConstants.COLLECT_FRUITS_EFFECT);
        collectEff.SetActive(true);
        collectEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
