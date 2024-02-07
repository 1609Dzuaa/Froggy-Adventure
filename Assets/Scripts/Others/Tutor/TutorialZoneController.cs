using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoneController : GameObjectManager
{
    [SerializeField] GameObject _tutorText;
    [SerializeField] bool _canDestroy;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        if (_canDestroy)
            EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.TutorOnDestroy, SelfDestroy);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        _tutorText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
            _tutorText.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            _tutorText.SetActive(true);
            //Debug.Log("Tutor !Popup");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            _tutorText.SetActive(false);
            //Debug.Log("Tutor !Popup");
        }
    }

    private void SelfDestroy(object obj)
    {
        GameObject gObj = (GameObject)obj;
        if (gObj == gameObject)
        {
            PlayerPrefs.SetString(GameEnums.ESpecialStates.Deleted + _ID, "deleted");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.TutorOnDestroy, SelfDestroy);
    }
}
