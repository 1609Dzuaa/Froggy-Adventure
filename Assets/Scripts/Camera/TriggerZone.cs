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

    [Header("Player Ref")]
    [SerializeField] private Transform _playerRef;

    private bool _hasPlayed;

    //Nên check = overlapBox vì có thể nếu Player jump vào cái vùng trigger thì sẽ 0 bị trigger
    //(Do 0 thoả mãn ở trên Ground)

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
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        return Physics2D.OverlapBox(transform.position, _sizeZone, 0f, _playerLayer) && playerScript.GetIsOnGround() && !_hasPlayed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, _sizeZone);   
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            var playerScript = collision.GetComponent<PlayerStateManager>();
            if (playerScript.GetIsOnGround())
            {
                _playableDirector.Play();
                Debug.Log("Trigger Play");
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }*/
}
