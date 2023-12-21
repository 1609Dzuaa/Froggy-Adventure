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
            Debug.Log("Trigger Play");
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
