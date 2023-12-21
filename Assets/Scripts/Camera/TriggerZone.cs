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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            _playableDirector.Play();
            Debug.Log("Trigger Play");
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
