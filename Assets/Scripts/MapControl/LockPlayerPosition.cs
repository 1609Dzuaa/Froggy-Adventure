using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPlayerPosition : MonoBehaviour
{
    [Header("Mỗi level cần ref con player")]
    [SerializeField] Transform _player;

    [Header("Level Boundaries")]
    [SerializeField] float _minBound;
    [SerializeField] float _maxBound; //1 vài level sẽ 0 cần tới thằng này

    // Update is called once per frame
    void Update()
    {
        if (_player.position.x < _minBound)
            _player.position = new Vector3(_minBound, _player.position.y, _player.position.z);
        else if (_player.position.x > _maxBound && _maxBound != 0)
            _player.position = new Vector3(_maxBound, _player.position.y, _player.position.z);
    }
}
