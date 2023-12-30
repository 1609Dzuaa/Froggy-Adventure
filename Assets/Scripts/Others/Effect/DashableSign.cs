using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashableSign : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = PlayerStateManager.PlayerInstance.DashableSignPosition.position; 
    }

    void Update()
    {
        transform.position = PlayerStateManager.PlayerInstance.DashableSignPosition.position;
    }
}
