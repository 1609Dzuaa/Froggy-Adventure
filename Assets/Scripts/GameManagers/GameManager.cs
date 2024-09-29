using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static GameConstants;
using static GameEnums;

public class GameManager : BaseSingleton<GameManager>
{
    bool _isReplay; //Nếu replay thì chờ loadscene r hẵng restart HP
    bool _fullUnlock;
    bool _deleteScene1Data;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchScene(int sceneIndex, bool canTweenImmediately)
    {
        //Nên tối giản func này chỉ làm đúng nhiệm vụ chuyển scene
        //UIManager sẽ lo hết tween, togglepopup
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void SwitchNextScene()
    {
        //StartCoroutine(SwitchScene(SceneManager.GetActiveScene().buildIndex + 1));
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.ButtonSelectedSfx, 1.0f);
        //StartCoroutine(PlayNextSceneSong(SceneManager.GetActiveScene().buildIndex + 1, true));
        ResetGameData();
        //OnClick của button "Play"
    }

    /*private IEnumerator PlayNextSceneSong(int index, bool needWait)
    {
        if (needWait)
            yield return new WaitForSeconds(_delayPlayThemeMusic);

        switch (index)
        {
            case 0:
                SoundsManager.Instance.PlayMusic(GameEnums.ESoundName.StartMenuTheme);
                break;

            case 1:
                SoundsManager.Instance.PlayMusic(GameEnums.ESoundName.Level1Theme);
                break;

            case 2:
                SoundsManager.Instance.PlayMusic(GameEnums.ESoundName.Level2Theme);
                Debug.Log("here");
                break;
        }
    }*/

    private void ResetGameData()
    {
        PlayerHealthManager.Instance.DecreaseHP();
        PlayerHealthManager.Instance.RestartHP();
        PlayerPrefs.DeleteAll();
        _deleteScene1Data = false;
        //Debug.Log("reset GData");
    }

    public void SwitchToScene(int sceneIndex)
    {
        //StartCoroutine(SwitchScene(sceneIndex));
    }

    /*public IEnumerator SwitchScene(int sceneIndex)
    {
        UIManager.Instance.IncreaseTransitionCanvasOrder();
        UIManager.Instance.PopDownAllPanels();
        UIManager.Instance.TriggerAnimation(GameConstants.SCENE_TRANS_END);

        yield return new WaitForSeconds(_delayTrans);

        //Nếu đang là scene 1 và scene tới là scene 2 thì cần xoá hết data
        if (SceneManager.GetActiveScene().buildIndex == 1 && sceneIndex == 2)
            _deleteScene1Data = true;

        SceneManager.LoadSceneAsync(sceneIndex);
        UIManager.Instance.TriggerAnimation(GameConstants.SCENE_TRANS_START);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.SceneEntrySfx, 1.0f);
        if (_isReplay)
        {
            _isReplay = false;
            PlayerHealthManager.Instance.RestartHP();
            Debug.Log("Replay");
            if (SoundsManager.Instance.IsPlayingBossTheme)
                SoundsManager.Instance.PlayMusic(GameEnums.ESoundName.Level2Theme);
            //Nếu là Replay thì restart HP
        }
        else if (sceneIndex == GameConstants.GAME_LEVEL_2)
        {
            if (PlayerPrefs.HasKey(GameEnums.ESpecialStates.PlayerSkillUnlockedLV2.ToString()))
                _fullUnlock = true;
            else
                _fullUnlock = false;
            //prob here
            if (_deleteScene1Data)
            {
                _deleteScene1Data = false; //xoá xong thì trả về false
                PlayerPrefs.DeleteAll();
                StartCoroutine(PlayNextSceneSong(sceneIndex, false));
                Debug.Log("PlaySong: " + sceneIndex);
            }
            PlayerPrefs.SetString((_fullUnlock) ? GameEnums.ESpecialStates.PlayerSkillUnlockedLV2.ToString() : GameEnums.ESpecialStates.PlayerSkillUnlockedLV1.ToString(), "Unlocked");
            PlayerPrefs.Save();

            PlayerHealthManager.Instance.IncreaseHP();
        }
        //Nếu là chuyển scene 2 thì có 2 lần check là đã full skills ch
        //và có cần xoá hết data nếu scene trc là scene 1 kh
    }*/

    public void ReloadScene()
    {
        Time.timeScale = 1.0f;
        _isReplay = true;
        SwitchToScene(SceneManager.GetActiveScene().buildIndex);
        //OnClick của button "Replay"
        //Chơi lại scene này (start tại vị trí flag gần nhất)
    }

    public void BackHome()
    {
        SceneManager.LoadSceneAsync(GameConstants.GAME_MENU);
        //StartCoroutine(PlayNextSceneSong(GameConstants.GAME_MENU, false));
        //OnClick của button "Home"
    }

    public void Restart()
    {
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.ObjectOnRestart, null);
        ResetGameData();
        //StartCoroutine(PlayNextSceneSong(GameConstants.GAME_LEVEL_1, true));
        SwitchToScene(GameConstants.GAME_LEVEL_1);
        //OnClick của button "Restart"
        //Chơi lại từ đầu
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteAll();
        PlayerHealthManager.Instance.DecreaseHP();
        //Debug.Log("Quit");
    }

}
