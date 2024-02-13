using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Xử lý UI nâng cấp skill: Mỗi khi unlock đc skills thì Popup UI skills achieved lên
/// Thông qua notify kèm theo tham số là tên skill, sau đó bên UI sẽ nhận và hiển thị UI
/// skill dựa trên tham số đó
/// UIManager sẽ chứa 1 ref gameobject là SkillAchievedPanel, 1 text tên skill, 1 Img ảnh skill 
/// Kèm thêm các dòng Text:
/// Skill Unlocked:
/// Tên Skill và ảnh skill ở phía dưới
/// 
/// Build Level 2 và Boss ở cuối Level
/// Build nốt state spawn Particle cho Boss, xử lý việc chuyển state của boss và random skill (?)
/// Gecko có bung ?
/// Xong boss, còn build map lv2 và bố trí Room Boss là end game
/// Qua lv2 thì xoá hết data lv1 (quái, checkpoint) giữ skill player
/// Vẫn còn bug quái có thể detect ra Player dù ở bên kia wall nhưng
/// chấp nhận 0 sửa (0 còn time)
/// Gđ cuối r nên chấp nhận code bẩn, 0 còn time refactor
/// Sửa lại state cho Boss chờ Player và Timeline, chỉnh lại camera 1 số chỗ đặc biệt và build game
/// </summary>

public class GameManager : BaseSingleton<GameManager>
{
    [SerializeField] float _delayTrans;
    [SerializeField] float _delayPlayThemeMusic;
    bool _isReplay; //Nếu replay thì chờ loadscene r hẵng restart HP

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchNextScene()
    {
        UIManager.Instance.IncreaseTransitionCanvasOrder();
        StartCoroutine(SwitchScene(SceneManager.GetActiveScene().buildIndex + 1));
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.ButtonSelectedSfx, 1.0f);
        StartCoroutine(PlayNextSceneSong(SceneManager.GetActiveScene().buildIndex + 1, true));
        ResetGameData();
        //OnClick của button "Play"
    }

    private IEnumerator PlayNextSceneSong(int index, bool needWait)
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
                break;
        }
    }

    private void ResetGameData()
    {
        PlayerHealthManager.Instance.DecreaseHP();
        PlayerHealthManager.Instance.RestartHP();
        PlayerPrefs.DeleteAll();
    }

    public void SwitchToScene(int sceneIndex)
    {
        UIManager.Instance.IncreaseTransitionCanvasOrder();
        UIManager.Instance.PopDownHPCanvas();
        StartCoroutine(SwitchScene(sceneIndex));
    }

    public IEnumerator SwitchScene(int sceneIndex)
    {
        UIManager.Instance.IncreaseTransitionCanvasOrder();
        UIManager.Instance.PopDownAllPanels();
        UIManager.Instance.TriggerAnimation(GameConstants.SCENE_TRANS_END);

        yield return new WaitForSeconds(_delayTrans);

        SceneManager.LoadSceneAsync(sceneIndex);
        UIManager.Instance.TriggerAnimation(GameConstants.SCENE_TRANS_START);
        SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.SceneEntrySfx, 1.0f);
        if (_isReplay)
        {
            _isReplay = false;
            PlayerHealthManager.Instance.RestartHP();
        }
        else if (sceneIndex == GameConstants.GAME_LEVEL_2)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString(GameEnums.ESpecialStates.PlayerSkillUnlockedLV1.ToString(), "Unlocked");
            PlayerHealthManager.Instance.IncreaseHP();
        }
        //Nếu là Replay thì restart HP, nếu là chuyển scene 2
        //thì xoá mọi data trừ Player's skills, tăng HP lên 
    }

    public void ReloadScene()
    {
        Time.timeScale = 1.0f;
        UIManager.Instance.PopDownAllPanels();
        _isReplay = true;
        SwitchToScene(SceneManager.GetActiveScene().buildIndex);
        //OnClick của button "Replay"
        //Chơi lại scene này (start tại vị trí flag gần nhất)
    }

    public void BackHome()
    {
        UIManager.Instance.PopDownAllPanels();
        UIManager.Instance.StartMenuCanvas.SetActive(true);
        SceneManager.LoadSceneAsync(GameConstants.GAME_MENU);
        StartCoroutine(PlayNextSceneSong(GameConstants.GAME_MENU, false));
        //OnClick của button "Home"
    }

    public void Restart()
    {
        UIManager.Instance.PopDownAllPanels();
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.ObjectOnRestart, null);
        ResetGameData();
        SwitchToScene(GameConstants.GAME_LEVEL_1);
        //OnClick của button "Restart"
        //Chơi lại từ đầu
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerHealthManager.Instance.DecreaseHP();
        Debug.Log("Quit");
    }

}
