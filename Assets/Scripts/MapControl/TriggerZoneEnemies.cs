using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

/// <summary>
/// Là 1 vùng vô hình, player đi qua sẽ active quái trong khu vực
/// </summary>
public class TriggerZoneEnemies : MonoBehaviour
{
    [SerializeField] Transform[] _listEnemies;
    bool _active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG) && !_active)
        {
            _active = true;
            foreach (Transform e in _listEnemies)
                e.gameObject.SetActive(true);
        }
    }
}
