using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialZoneController : MonoBehaviour
{
    [SerializeField] GameObject _tutorText;
    [SerializeField, Tooltip("Nếu có thì tick vào," +
        " 0 thì 0 quan tâm cái này và cái dưới")] bool _hasLimitTime;
    [SerializeField] float _limitTime;
    private float _entryTime;
    private bool _hasTick;

    // Start is called before the first frame update
    void Start()
    {
        _tutorText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _entryTime >= _limitTime && _hasLimitTime && _hasTick)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            _tutorText.SetActive(true);
            if (!_hasTick && _hasLimitTime)
            {
                _hasTick = true;
                _entryTime = Time.time;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            _tutorText.SetActive(true);
            Debug.Log("Tutor !Popup");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            _tutorText.SetActive(false);
            Debug.Log("Tutor !Popup");
        }
    }
}
