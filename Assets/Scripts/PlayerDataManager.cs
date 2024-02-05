using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    Transform _playerRef;

    // Start is called before the first frame update
    void Awake()
    {
        _playerRef = GameObject.FindWithTag(GameConstants.PLAYER_TAG).transform;
        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            _playerRef.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
        }
    }

    private void OnEnable()
    {
        EventsManager.Instance.SubcribeToAnEvent(GameEnums.EEvents.PlayerOnUpdateRespawnPosition, UpdateRespawnPosition);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(GameEnums.EEvents.PlayerOnUpdateRespawnPosition, UpdateRespawnPosition);
        //Unsub tránh bị null ref
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateRespawnPosition(object obj)
    {
        PlayerPrefs.SetFloat("PlayerPosX", _playerRef.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", _playerRef.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", _playerRef.position.z);
        PlayerPrefs.Save();
        Debug.Log("pos saved x, y, z: " + PlayerPrefs.GetFloat("PlayerPosX") + " " + PlayerPrefs.GetFloat("PlayerPosY") + " " + PlayerPrefs.GetFloat("PlayerPosZ"));
    }
}
