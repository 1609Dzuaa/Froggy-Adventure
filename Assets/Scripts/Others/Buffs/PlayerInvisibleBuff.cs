using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisibleBuff : MonoBehaviour
{
    [SerializeField] Transform _playerRef;
    [SerializeField] private float _alphaApply;
    [SerializeField] private float _duration;

    private static PlayerInvisibleBuff _invisibleBuffInstance;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _isAllowToUpdate;
    private float _entryTime;

    public static PlayerInvisibleBuff Instance
    {
        get
        {
            if (_invisibleBuffInstance == null)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _invisibleBuffInstance = FindObjectOfType<PlayerInvisibleBuff>();

                if (!_invisibleBuffInstance)
                    Debug.Log("No InvisibleBuff in scene");
            }
            return _invisibleBuffInstance;
        }
    }
    private void Awake()
    {
        CreateInstance();
        _playerSpriteRenderer = _playerRef.GetComponent<SpriteRenderer>();
    }

    private void CreateInstance()
    {
        if (!_invisibleBuffInstance)
        {
            _invisibleBuffInstance = this;
            DontDestroyOnLoad(gameObject); //Đảm bảo thằng này 0 bị huỷ khi chuyển Scene
            //Docs: Don't destroy the target Object when loading a new Scene.
        }
        else
        {
            //Nếu đã tồn tại thằng Instance != ở trong game thì destroy thằng này
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isAllowToUpdate)
        {
            if (Time.time - _entryTime >= _duration)
            {
                _isAllowToUpdate = false;
                _playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                Debug.Log("Timeout!");
            }
        }
    }

    public void ApplyBuff()
    {
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        _playerSpriteRenderer.color = new Color(1f, 1f, 1f, _alphaApply);
    }

}