using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Xử lý UI khi loose, win game
/// Xử lý UI nâng cấp skill (?)
/// Xử lý Data Player khi chết
/// Build Level 2 và Boss ở cuối Level
/// Build Boss cơ bản = state pattern, bỏ qua Behavior Tree vì 0 còn thgian
/// Xem lại vài đoạn setactive cũng như UI HP
/// Thêm thông tin các button ở phần infor (?)
/// Bug từ GH => Dead 
/// </summary>

public class GameManager : BaseSingleton<GameManager>
{
    [SerializeField] float _delayTrans;

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
        PlayerHealthManager.Instance.RestartHP();
        PlayerPrefs.DeleteAll();
        //OnClick của button "Play"
    }

    public void SwitchToScene(int sceneIndex, bool needPopDownHP)
    {
        UIManager.Instance.IncreaseTransitionCanvasOrder();
        if (needPopDownHP)
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
    }

    public void ReloadScene()
    {
        Time.timeScale = 1.0f;
        UIManager.Instance.PopDownAllPanels();
        PlayerHealthManager.Instance.RestartHP();
        SwitchToScene(SceneManager.GetActiveScene().buildIndex, false);
        //OnClick của button "Replay"
        //Chơi lại scene này (start tại vị trí flag gần nhất ?)
    }

    public void BackHome()
    {
        UIManager.Instance.PopDownAllPanels();
        UIManager.Instance.StartMenuCanvas.SetActive(true);
        SceneManager.LoadSceneAsync(0);
        //OnClick của button "Home"
    }

    public void Restart()
    {
        UIManager.Instance.PopDownAllPanels();
        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.ObjectOnRestart, null);
        PlayerHealthManager.Instance.RestartHP();
        PlayerPrefs.DeleteAll();
        SwitchToScene(1, true);
        //OnClick của button "Restart"
        //Chơi lại từ đầu
    }

}
