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

    protected override void Awake()
    {
        base.Awake();
        PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
        //Mobile platforms always ignore QualitySettings.vSyncCount and instead
        //use Application.targetFrameRate to choose a target frame rate for the game.
        Application.targetFrameRate = _targetFrameRate;
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
        //PlayerPrefs.DeleteAll();
        PlayerHealthManager.Instance.DecreaseHP();
        //Debug.Log("Quit");
    }

}
