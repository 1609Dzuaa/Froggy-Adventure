using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct HP
{
    public int _state;
    public Sprite _HPSprite;
    public Dictionary<int, Sprite> _dictHP; //Key là state, tương ứng là sprite của state đó
}

public class PlayerHealthController : MonoBehaviour
{
    //Class này dùng Quản lý HP và phụ trách render HP lên UI

    [Header("HP Icon")]
    [SerializeField] private Image[] _uiHP = new Image[10];
    [SerializeField] private Sprite _normalHPSprite;
    [SerializeField] private Sprite _lostHPSprite;
    [SerializeField] private Sprite _tempHPSprite;

    [Header("SO")]
    [SerializeField] private PlayerStats _playerSO;

    private static PlayerHealthController _Instance;
    private HP[] _HPs = new HP[7];
    private int _maxHP;
    private int _currentHP;
    private int _tempHP;

    public int TempHP { get { return _tempHP; } set { _tempHP = value; } }

    public static PlayerHealthController Instance
    {
        get
        {
            if (!_Instance)
            {
                //Tìm xem có Instance có trong Scene kh ?
                _Instance = FindObjectOfType<PlayerHealthController>();

                if (!_Instance)
                    Debug.Log("No HP Controller in scene");
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        _maxHP = _playerSO.MaxHP;
        _currentHP = _maxHP;
        _tempHP = 0;
        InitHPArray();
        InitHPDictionary();
    }

    private void CreateInstance()
    {
        if (!_Instance)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject); //Đảm bảo thằng này 0 bị huỷ khi chuyển Scene
            //Docs: Don't destroy the target Object when loading a new Scene.
        }
        else
        {
            //Nếu đã tồn tại thằng Instance != ở trong game thì destroy thằng này
            Destroy(gameObject);
        }
    }

    private void InitHPDictionary()
    {
        for (int i = 0; i < _maxHP; i++)
        {
            _HPs[i]._dictHP = new Dictionary<int, Sprite>()
            {
                {GameConstants.HP_STATE_NORMAL, _normalHPSprite },
                {GameConstants.HP_STATE_LOST, _lostHPSprite },
                {GameConstants.HP_STATE_TEMP, _tempHPSprite }
            };        
        }
        //Thiết lập từng key - value cho từng HP
    }

    private void InitHPArray()
    {
        for (int i = 0; i < _maxHP; i++)
        {
            _HPs[i]._state = GameConstants.HP_STATE_NORMAL;
            //Khởi tạo cho các HP mặc định là state Normal
        }
    }

    private void Update()
    {
        UpdateHPToUI();
        Debug.Log("TempHP: " + _tempHP);
    }

    public void ChangeHPState(int state)
    {
        HandleIfLostHP(state);

        if (state == GameConstants.HP_STATE_NORMAL)
        {
            _HPs[_currentHP]._state = state;
            if (_currentHP < _maxHP)
                _currentHP++;
        }
        else if (state == GameConstants.HP_STATE_TEMP)
        {
            if (_tempHP < _currentHP)
                _tempHP = _currentHP;
            _HPs[_tempHP]._state = state;
            _tempHP++;
        }
        //Buff hấp thụ thì 0 cần quan tâm maxHP, cộng cho dư luôn
        //GH thì state thành Lost
        //Increase thì state của thằng HPpara + 1 thành Normal
        //Temp thì state của thằng HPpara + 1 thành Temp
        //Gọi thằng này thông qua Instance mỗi khi có sự thay đổi về HP
        //(GH, Ăn HP, Ăn TempHP)
        Debug.Log("Cur HP: " + _currentHP);
    }

    private void HandleIfLostHP(int state)
    {
        if (state == GameConstants.HP_STATE_LOST)
        {
            if (_tempHP != 0)
                _tempHP--;
            else
                _currentHP--;
            _HPs[_currentHP]._state = state;
        }
    }

    private void UpdateHPToUI()
    {
        if (_tempHP < _maxHP)
        {
            for (int i = 0; i < _maxHP; i++)
            {
                if (!_uiHP[i])
                {
                    Debug.Log("NULLLL");
                    return;
                }
                _uiHP[i].sprite = _HPs[i]._dictHP[_HPs[i]._state];
            }
        }
        else
        {
            for (int i = 0; i <= _tempHP; i++)
            {
                if (_tempHP == 5)
                    Debug.Log("Tao o day");
                if (!_uiHP[i])
                {
                    Debug.Log("UI NULLLL");
                    return;
                }

                if (_HPs[i]._dictHP == null)
                {
                    _HPs[i]._dictHP = new Dictionary<int, Sprite>()
                    {
                        {GameConstants.HP_STATE_NORMAL, _normalHPSprite },
                        {GameConstants.HP_STATE_LOST, _lostHPSprite },
                        {GameConstants.HP_STATE_TEMP, _tempHPSprite }
                    };
                }
                _uiHP[i].sprite = _HPs[i]._dictHP[_HPs[i]._state];
            }
        }
        //Render lượng HP max lên sprite dựa trên state của nó 
    }
}
