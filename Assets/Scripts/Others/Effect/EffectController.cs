using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    //Vị trí mà mình muốn effect xuất hiện
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
