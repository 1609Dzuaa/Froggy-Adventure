using UnityEngine;

public class SwitchController : GameObjectManager
{
    bool _hasTriggered;
    // Start is called before the first frame update
    protected override void HandleObjectState()
    {
        _ID = gameObject.name;

        if (!PlayerPrefs.HasKey(_ID))
        {
            PlayerPrefs.SetString(_ID, _ID);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey(GameEnums.ESpecialStates.Disabled + _ID))
        {
            _anim.SetTrigger("Hit");
            _hasTriggered = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasTriggered)
        {
            _anim.SetTrigger("Hit");
            _hasTriggered = true;
            EventsManager.NotifyObservers(GameEnums.EEvents.FanOnBeingDisabled, null);
            SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.SwitchActivatedSfx, 1.0f);
            string key = GameEnums.ESpecialStates.Disabled + _ID;
            PlayerPrefs.SetString(key, "Disabled");
            GameManager.Instance.ListPrefsInconsistentKeys.Add(key);
        }
    }
}
