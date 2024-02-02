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

    PlayerStateManager _playerRef;
    BoxCollider2D _boxCol;

    private void Start()
    {
        _playerRef = FindObjectOfType<PlayerStateManager>();
        _boxCol = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameConstants.PLAYER_TAG) && _playerRef.GetIsOnGround())
        {
            _boxCol.enabled = false;
            _playableDirector.Play();
            Debug.Log("PlayEnter");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && _playerRef.GetIsOnGround())
        {
            _boxCol.enabled = false;
            _playableDirector.Play();
            Debug.Log("PlayStay");
        }
    }
}
