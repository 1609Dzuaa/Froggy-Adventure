using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoneController : GameObjectManager
{
    [SerializeField] GameObject _tutorText;
    [SerializeField] bool _canDestroy;
    bool _allowPopUp;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        if (_canDestroy)
            EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.TutorOnDestroy, SelfDestroy);
        //if (_isApplySkillToPlayer)
            //EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.OnUnlockSkill, AllowPopUpTutor);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        _tutorText.SetActive(false);
        /*if (_isApplySkillToPlayer)
            if (PlayerPrefs.HasKey((GameEnums.ESpecialStates.Actived + ID)))
                _allowPopUp = true;*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            if (!_isApplySkillToPlayer)
                _tutorText.SetActive(true);
            else
                _tutorText.SetActive((_allowPopUp) ? true : false);
            //Debug.Log("Tutor !Popup");
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            if (!_isApplySkillToPlayer)
                _tutorText.SetActive(true);
            else
                _tutorText.SetActive((_allowPopUp) ? true : false);
            //Debug.Log("Tutor !Popup");
        }*/
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
        EventsManager.Instance.UnsubscribeToAnEvent(GameEnums.EEvents.TutorOnDestroy, SelfDestroy);
        EventsManager.Instance.UnsubscribeToAnEvent(GameEnums.EEvents.OnUnlockSkill, AllowPopUpTutor);
    }

    /// <summary>
    /// Với 1 vài tutor hướng dẫn Player sau khi Unlock skill thì
    /// phải chờ cái Event Unlock skill đc notify thì mới popup
    /// </summary>

    private void AllowPopUpTutor(object obj)
    {
        //if ((GameEnums.EPlayerState)obj != _skillUnlocked)
            //return;

        _allowPopUp = true;
        PlayerPrefs.SetString(GameEnums.ESpecialStates.Actived + ID, "Unlocked");
    }
}
