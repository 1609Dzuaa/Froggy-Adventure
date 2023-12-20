using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerZone : MonoBehaviour
{
    //Object vô hình này sẽ là điểm mà khi Player trigger nó,
    //cam sẽ tự động điều chỉnh vị trí theo ý mình muốn

    [SerializeField] private PlayableDirector _playableDirector;

    private void Awake()
    {
        if (_playableDirector)
            Debug.Log("Tao deo null1");
    }

    private void Start()
    {
        if (_playableDirector)
            Debug.Log("Tao deo null2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (!_playableDirector)
            {
                Debug.Log("Tao bi null");
                return;
            }
            _playableDirector.Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    /*[SerializeField] private Transform _targetNeedToFollow;
    [SerializeField] private bool _canDisablePlayer;
    //[SerializeField] private PlayableDirector _playableDirector;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !CameraController.GetInstance().HasTriggered)
        {
            /*_playableDirector.enabled = true;
            _playableDirector.Play();
            if(_canDisablePlayer)
            {
                var PlayerScript = collision.GetComponent<PlayerStateManager>();
                PlayerScript.Disable();
                Debug.Log("Dis");
            }
            CameraController.GetInstance().HasTriggered = true;
            CameraController.GetInstance().HasLeavedTriggerZone = false;
            CameraController.GetInstance().TargetNeedToFollow = _targetNeedToFollow;
            //GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log("Trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && CameraController.GetInstance().HasTriggered)
        {
            CameraController.GetInstance().HasTriggered = false;
            CameraController.GetInstance().HasLeavedTriggerZone = true;
            Debug.Log("Leave");
        }
    }*/
}
