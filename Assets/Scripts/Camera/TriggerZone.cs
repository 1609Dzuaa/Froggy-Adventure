using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerZone : GameObjectManager
{
    //Object vô hình này sẽ là điểm mà khi Player trigger nó,
    //cam sẽ tự động điều chỉnh vị trí theo ý mình muốn

    [Header("Timeline Reference")]
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private GameObject _player;
    [SerializeField] bool _isBossTimeline;
    [SerializeField] float _delayCloseGate;
    PlayerStateManager _playerRef;
    BoxCollider2D _boxCol;

    protected override void Start()
    {
        _playerRef = _player.GetComponent<PlayerStateManager>();
        _boxCol = GetComponent<BoxCollider2D>();
    }

    protected override void HandleObjectState()
    {
        base.HandleObjectState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameConstants.PLAYER_TAG) && _playerRef.GetIsOnGround())
        {
            _boxCol.enabled = false;
            _playableDirector.Play();
            PlayerPrefs.SetString(GameEnums.ESpecialStates.Deleted + _ID, "Deleted");
            //Debug.Log("PlayEnter");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG)) //&& _playerRef.GetIsOnGround())
        {
            _boxCol.enabled = false;
            _playableDirector.Play();
            if (_isBossTimeline)
                SoundsManager.Instance.PlayMusic(GameEnums.ESoundName.BossTheme);
            //Debug.Log("PlayStay");
        }
    }
}
