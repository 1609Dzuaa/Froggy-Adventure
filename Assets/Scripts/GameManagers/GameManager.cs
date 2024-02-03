using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Xử lý UI khi loose, win game
/// Xử lý UI nâng cấp skill
/// Xử lý Data Player khi chết
/// Build Level 2 và Boss ở cuối Level
/// Build Boss cơ bản = state pattern, bỏ qua Behavior Tree vì 0 còn thgian
/// </summary>

public class GameManager : BaseSingleton<GameManager>
{
    [SerializeField] float _delayTrans;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void ReloadScene()
    {
        //Popdown các panel

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SwitchNextScene()
    {
        UIManager.Instance.ChangeTransitionCanvasOrder();
        StartCoroutine(SwitchToNextScene());
    }

    public IEnumerator SwitchToNextScene()
    {
        UIManager.Instance.PopDownCreditsPanel();
        UIManager.Instance.PopDownSettingsPanel();
        UIManager.Instance.TriggerAnimation(GameConstants.SCENE_TRANS_END);

        yield return new WaitForSeconds(_delayTrans);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        UIManager.Instance.TriggerAnimation(GameConstants.SCENE_TRANS_START);
    }
}
