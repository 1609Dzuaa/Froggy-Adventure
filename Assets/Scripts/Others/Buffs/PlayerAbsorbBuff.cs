using UnityEngine;

public class PlayerAbsorbBuff : MonoBehaviour
{
    //Nên bố trí vị trí buff này sao cho
    //Khoảng thgian ăn buff 1 và buff 2 không gần kề nhau tránh bug (có thể có)

    [SerializeField] private float _buffDuration;
    [SerializeField] private float _tempHPDuration;
    [SerializeField] private float _tempHPRunOutDuration;
    [SerializeField] private Transform _tempShieldIcon; //sign báo hiệu vẫn còn thgian buff

    private static PlayerAbsorbBuff _absurbBuffInstance;
    private bool _isAllowToUpdate;
    private float _entryTime;

    //Thêm hiệu ứng để biết hết Buff tương tự như Shield ?

    public float TempHPDuration { get { return _tempHPDuration; } }

    public float TempHPRunOutDuration { get { return _tempHPRunOutDuration; } }

    public static PlayerAbsorbBuff Instance
    {
        get
        {
            if (_absurbBuffInstance == null)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _absurbBuffInstance = FindObjectOfType<PlayerAbsorbBuff>();

                if (!_absurbBuffInstance)
                    Debug.Log("No Absorb Buff in scene");
            }
            return _absurbBuffInstance;
        }
    }

    public bool IsAllowToUpdate { get { return _isAllowToUpdate; } }

    private void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        _tempShieldIcon.gameObject.SetActive(false);
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
            //Debug.Log("Updateee");
            if (Time.time - _entryTime >= _buffDuration)
            {
                _isAllowToUpdate = false;
                _tempShieldIcon.gameObject.SetActive(false);
            }
        }
    }

    public void ApplyBuff()
    {
        _isAllowToUpdate = true;
        _entryTime = Time.time;
        _tempShieldIcon.gameObject.SetActive(true);
    }
}
