using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashableSign : MonoBehaviour
{
    PlayerStateManager _playerRef;

    private void Start()
    {
        _playerRef = FindObjectOfType<PlayerStateManager>();
    }

    private void OnEnable()
    {
        if (_playerRef)
            transform.position = _playerRef.DashableSignPosition.position;
    }

    void Update()
    {
        if (_playerRef)
            transform.position = _playerRef.DashableSignPosition.position;
    }
}
