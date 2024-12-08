using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static GameConstants;
using static GameEnums;

public class GameManager : BaseSingleton<GameManager>
{
    [SerializeField] int _targetFrameRate;
    [HideInInspector] public List<string> ListPrefsInconsistentKeys;

    protected override void Awake()
    {
        base.Awake();
        //PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
        //Mobile platforms always ignore QualitySettings.vSyncCount and instead
        //use Application.targetFrameRate to choose a target frame rate for the game.
        Application.targetFrameRate = _targetFrameRate;
        ListPrefsInconsistentKeys = new List<string>();
    }

    public void SwitchScene(int sceneIndex)
    {
        //Nên tối giản func này chỉ làm đúng nhiệm vụ chuyển scene
        //UIManager sẽ lo hết tween, togglepopup
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        DeleteInconsistentPrefsKey();
        PlayerHealthManager.Instance.DecreaseHP();
        //Debug.Log("Quit");
    }

    public void DeleteInconsistentPrefsKey()
    {
        foreach (string key in ListPrefsInconsistentKeys)
            PlayerPrefs.DeleteKey(key);
    }
}
