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

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<GameManager>();

            if (!_instance)
                Debug.Log("0 co Game Manager trong Scene");

            return _instance;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SwitchToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
