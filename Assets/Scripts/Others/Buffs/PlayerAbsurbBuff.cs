using UnityEngine;

public class PlayerAbsurbBuff : MonoBehaviour
{
    [SerializeField] Transform _playerRef;
    [SerializeField] private float _duration;

    private static PlayerAbsurbBuff _absurbBuffInstance;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _isAllowToUpdate;
    private float _entryTime;

    public static PlayerAbsurbBuff Instance
    {
        get
        {
            if (_absurbBuffInstance == null)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _absurbBuffInstance = FindObjectOfType<PlayerAbsurbBuff>();

                if (!_absurbBuffInstance)
                    Debug.Log("No Absurb Buff in scene");
            }
            return _absurbBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } }

    private void Awake()
    {
        CreateInstance();
        _playerSpriteRenderer = _playerRef.GetComponent<SpriteRenderer>();
    }

    private void CreateInstance()
    {
        if (!_absurbBuffInstance)
        {
            _absurbBuffInstance = this;
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
                //Debug.Log("Timeout!");
            }
        }
    }

    public void ApplyBuff()
    {
        _entryTime = Time.time;
        
    }
}
