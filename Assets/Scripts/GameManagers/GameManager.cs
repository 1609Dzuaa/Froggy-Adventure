using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Có vấn đề chung với các Pool khi reload Scene, xem lại cách add
/// Cần thêm UI Manager (vì mỗi lần load lại scene thì dính bug _uiHP)
/// Load lại Scene, Dash thử 1 lần cũng dính bug ( Xxem lại việc add các vfx)
/// Reconstruct lại SoundManager
/// R sau đó hẵng xử lý các vấn đề liên quan data của Player
/// </summary>

public class GameManager : BaseSingleton<GameManager>
{
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

    public void SwitchToNextScene()
    {
        UIManager.Instance.PopDownCreditsPanel();
        UIManager.Instance.PopDownSettingsPanel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
