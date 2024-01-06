using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerZone : MonoBehaviour
{
    //Object vô hình này sẽ là điểm mà khi Player trigger nó,
    //cam sẽ tự động điều chỉnh vị trí theo ý mình muốn

    [Header("Timeline Reference")]
    [SerializeField] private PlayableDirector _playableDirector;

    [Header("Size Zone")]
    [SerializeField] private Vector2 _sizeZone;

    [Header("Player Layer")]
    [SerializeField] private LayerMask _playerLayer;

    private bool _hasPlayed;
    private PlayerStateManager _playerRef;

    private void Start()
    {
        _playerRef = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (CanPlayTimeline())
        {
            _hasPlayed = true;
            _playableDirector.Play();
        }
    }

    private bool CanPlayTimeline()
    {
        return Physics2D.OverlapBox(transform.position, _sizeZone, 0f, _playerLayer) && _playerRef.GetIsOnGround() && !_hasPlayed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, _sizeZone);   
    }
}
