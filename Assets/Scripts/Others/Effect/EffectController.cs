using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] GameEnums.EEfects _vfxType;
    [SerializeField, Tooltip("Một số Vfx có thể xuất hiện nhiều cùng 1 lúc " +
        "nên cân nhắc về số lượng của nó")]
    int _vfxAmmount;

    private void Start()
    {
        //Khởi đầu thì thêm cái vfx này vào scene (tương tự cho các bể khác)
        EffectPool.Instance.AddVFXToPool(gameObject, _vfxType, _vfxAmmount);
    }

    private void Deactive()
    {
        gameObject.SetActive(false);
        //Event của animation effect
    }

    //Vị trí mà mình muốn effect xuất hiện
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
