using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuffController : ItemsController
{
    [Header("Shield")]
    [SerializeField] private GameObject _shieldBuff;

    /*private void OnEnable()
    {
        PlayerStateManager.OnAppliedBuff += ApplyBuff;    
    }

    private void OnDisable()
    {
        PlayerStateManager.OnAppliedBuff -= ApplyBuff;
    }*/

    private void ApplyBuff()
    {
        _shieldBuff.SetActive(true);
        HandleAfterAppliedBuff();
    }

    private void HandleAfterAppliedBuff()
    {
        Instantiate(_collectedEffect, transform.position, Quaternion.identity, null);
        Destroy(this.gameObject);
    }
}
