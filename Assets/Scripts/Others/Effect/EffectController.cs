using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
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
