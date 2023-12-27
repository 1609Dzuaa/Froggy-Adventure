using UnityEngine;

public class PlayerAbsorbBuff : MonoBehaviour
{
    [SerializeField] private float _duration;

    private static PlayerAbsorbBuff _absurbBuffInstance;
    private bool _isAllowToUpdate;
    private float _entryTime;

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
            if (Time.time - _entryTime >= _duration)
            {
                _isAllowToUpdate = false;
                HandleIterateTempHP();
                PlayerHealthController.Instance.TempHP = 0; //Đưa temp về 0 sau khi duyệt xong
            }
        }
    }

    private void HandleIterateTempHP()
    {
        //Func này để duyệt và set giá trị các TempHP, cách duyệt:
        //Bắt đầu từ VỊ TRÍ sau VỊ TRÍ của currentHP, nếu tempHP != 0 <=> ĐK for dưới thoả mãn thì:
        //Nếu vị trí của i VƯỢT QUÁ vị trí của MaxHP thì disable nó đi
        //Nếu 0 thì gán lại state lost cho vị trí đó
        for (int i = PlayerHealthController.Instance.CurrentHP; i < PlayerHealthController.Instance.CurrentHP + PlayerHealthController.Instance.TempHP; i++)
        {
            if (i > PlayerHealthController.Instance.MaxHP - 1)
                PlayerHealthController.Instance.UIHPs[i].enabled = false;
            else
                PlayerHealthController.Instance.HPs[i]._state = GameConstants.HP_STATE_LOST;
            //Debug.Log("i, state: " + i + ", " + PlayerHealthController.Instance.HPs[i]._state); 
        }
    }

    public void ApplyBuff()
    {
        _isAllowToUpdate = true;
        _entryTime = Time.time;
    }
}
