using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameConstants;
using static GameEnums;

public struct HP
{
    public int _state;
    public Sprite _HPSprite;
    public Dictionary<int, Sprite> _dictHP; //Key là state, tương ứng là sprite của state đó
}

public class PlayerHealthManager : BaseSingleton<PlayerHealthManager>
{
    //Class này dùng Quản lý HP và phụ trách render HP lên UI

    [Header("HP Icon")]
    [SerializeField] private Image[] _uiHP = new Image[PLAYER_MAX_HP];
    [SerializeField] private Sprite _normalHPSprite;
    [SerializeField] private Sprite _lostHPSprite;
    [SerializeField] private Sprite _tempHPSprite;

    [Header("SO")]
    [SerializeField] private PlayerStats _playerSO;

    [Header("Time")]
    //khoảng thgian để blink máu ảo khi nó trong trạng thái RunningOut
    [SerializeField] private float _timeEachBlink;
    [SerializeField] float _tempHPDuration;
    [SerializeField] float _tempHPRunoutDuration;

    private HP[] _HPs = new HP[PLAYER_MAX_HP];
    [HideInInspector] public int MaxHP;
    private int _currentHP;
    private int _tempHP;
    private bool _hasIncreaseHP;

    //Giúp cảnh báo Player khi máu ảo sắp hết thgian sử dụng ^^
    #region BLINK EFFECT FOR TEMP_HP WHEN RUNNING OUT
    private bool _hasGotTempHP;
    private float _tempHPEntryTime;
    private bool _hasTickRunOut;
    private float _tempHPEachRunOutEntryTime;
    private float _tempHPRunOutEntryTime;
    private bool _blinkLost = true;
    #endregion

    public HP[] HPs { get { return _HPs; } set { HPs = value; } }

    public int CurrentHP { get { return _currentHP; } set { _currentHP = value; } }

    public int TempHP { get => _tempHP; set => _tempHP = value; }

    protected override void Awake()
    {
        base.Awake();
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnAidForPlayer, AidForPlayer);
    }

    private void AidForPlayer(object obj)
    {
        _currentHP = DEFAULT_AID_PLAYER_HP;
        _tempHP = DEFAULT_AID_PLAYER_TEMP_HP;
        _hasGotTempHP = true; //Đánh dấu đã nhận đc máu ảo để tính giờ
        _tempHPEntryTime = Time.time; //Bắt đầu tính giờ thgian để sd máu ảo
        AssignHPArray(); //assign lại cái HP UI
        Debug.Log("Aid");
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnSubcribeToAnEvent(EEvents.OnAidForPlayer, AidForPlayer);
    }

    private void Start()
    {
        InitHP();
        AssignHPArray();
        InitHPDictionary();
        InitUIHP();
    }

    private void InitHP()
    {
        //MaxHP = _playerSO.MaxHP;
        //MaxHP = DEFAULT_MAX_HP;
        //_tempHP = 0;
    }

    private void InitHPDictionary()
    {
        for (int i = 0; i < MaxHP; i++)
        {
            _HPs[i]._dictHP = new Dictionary<int, Sprite>()
            {
                {HP_STATE_NORMAL, _normalHPSprite },
                {HP_STATE_LOST, _lostHPSprite },
                {HP_STATE_TEMP, _tempHPSprite }
            };        
        }
        //Thiết lập từng key - value cho từng HP
    }

    private void InitUIHP()
    {
        for (int i = 0; i < PLAYER_MAX_HP; i++)
        {
            if (i < MaxHP)
                _uiHP[i].enabled = true;
            else
                _uiHP[i].enabled = false;
        }
        //Enable những thằng có index < maxHP
        //Disable những thằng còn lại
    }

    private void AssignHPArray()
    {
        for (int i = 0; i < MaxHP; i++)
        {
            if (i <= _currentHP - 1)
                _HPs[i]._state = HP_STATE_NORMAL;
            else if (_tempHP != 0)
                _HPs[i]._state = HP_STATE_TEMP;
            else
                _HPs[i]._state = HP_STATE_LOST;
            
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            HandleIterateTempHP();
            UpdateHPToUI();
        }
        //Debug.Log("temp: " + _tempHP);
        //Debug.Log("Curr, TempHP , state: " + _currentHP + ", " + _tempHP + "," + _HPs[_tempHP]._state);
    }

    public void ChangeHPState(int state)
    {
        
        HandleIfIncreaseHP(state);
        HandleIfDecreaseHP(state);
        HandleIfIncreaseTempHP(state);
        
        //Buff hấp thụ thì 0 cần quan tâm maxHP, cộng cho dư luôn
        //GH thì state thành Lost
        //Gọi thằng này thông qua Instance mỗi khi có sự thay đổi về HP
        //(GH, Ăn HP, Ăn TempHP)
        //Debug.Log("Cur HP: " + _currentHP);
    }

    public void HandleIfCurse()
    {
        for (int i = 0; i < _currentHP; i++)
            _HPs[i]._state = HP_STATE_LOST;
        _currentHP = 0;
    }

    private void HandleIfIncreaseHP(int state)
    {
        if (state == HP_STATE_NORMAL)
        {
            if (_currentHP < MaxHP)
            {
                _HPs[_currentHP]._state = state;
                _currentHP++;
            }
        }
        //Func này ổn r
    }

    private void HandleIfDecreaseHP(int state)
    {
        if (state == HP_STATE_LOST)
        {
            //Check có máu ảo thì trừ nó, 0 thì trừ thẳng hp hiện tại
            if (_tempHP != 0)
            {
                if (_tempHP + _currentHP > MaxHP)
                {
                    //Minus 1 bcuz it's a array
                    //Nếu tổng máu ảo + máu real vượt quá maxHP thì disable máu ảo 
                    _uiHP[_tempHP + _currentHP - 1].enabled = false;
                    _tempHP--;
                }
                else
                {
                    //Nếu tổng máu ảo + máu real <= maxHP thì set cái state của máu ảo về LOST 
                    _HPs[_tempHP + _currentHP - 1]._state = state;
                    _tempHP--;
                }
            }
            else
            {
                if (_currentHP > 0)
                {
                    _currentHP--;
                    //Debug.Log("Cur: " + _currentHP);
                }
                
                _HPs[_currentHP]._state = state;
            }
            //Debug.Log("current, state: " + _currentHP + ", " + _HPs[_currentHP]._state);
        }
    }

    private void HandleIfIncreaseTempHP(int state)
    {
        if (state == HP_STATE_TEMP)
        {
            if (_tempHP == 0)
            {
                _hasGotTempHP = true; //Đánh dấu đã nhận đc máu ảo để tính giờ
                _tempHPEntryTime = Time.time; //Bắt đầu tính giờ thgian để sd máu ảo
            }

            //Set state máu ảo cho các HP từ máu hiện tại trở đi
            _HPs[_currentHP + _tempHP]._state = state;
            _tempHP++;

            //Sau khi đã tăng máu ảo thì check nếu tổng máu hiện tại + máu ảo >= maxHP
            //thì cho phép các thanh máu sau maxHP enable
            if (_tempHP + _currentHP >= MaxHP)
                _uiHP[_tempHP + _currentHP - 1].enabled = true;
        }
        //Ổn
    }

    private void UpdateHPToUI()
    {
        //Tách 2 phần, Lượng máu đc hiển thị trên UI tính theo:
        //Máu hiện tại + máu ảo (nếu có):
        //1. Loop đến max, render từng phần máu tuỳ theo state của nó
        //2. Loop đến máu ảo hiện tại, render từng phần máu

        //Loop đến max
        for (int i = 0; i < MaxHP; i++)
        {
            _uiHP[i].sprite = _HPs[i]._dictHP[_HPs[i]._state];
            //Debug.Log("Curr, TempHP , Curr_state: " + _currentHP + ", " + _tempHP + "," + _HPs[_currentHP]._state);
        }

        //Loop đến Temp
        for (int i = 0; i < _tempHP; i++)
        {
            //Thêm dòng này vì có thể tempHP vượt quá max dẫn đến dictionary bị null
            if (_HPs[i + _currentHP]._dictHP == null)
            {
                _HPs[i + _currentHP]._dictHP = new Dictionary<int, Sprite>()
                {
                        { HP_STATE_NORMAL, _normalHPSprite },
                        { HP_STATE_LOST, _lostHPSprite },
                        { HP_STATE_TEMP, _tempHPSprite }
                };
            }
            //Debug.Log("Co vao day");
            _uiHP[i + _currentHP].sprite = _HPs[i + _currentHP]._dictHP[_HPs[i + _currentHP]._state];
            
            //Render lượng HP max lên sprite dựa trên state của nó 
        }
    }

    private void HandleIterateTempHP()
    {
        //Nếu hết thgian sử dụng tempHP
        if (Time.time - _tempHPEntryTime >=  _tempHPDuration && _hasGotTempHP)
        {
            //Debug.Log("TempHP Ready Run Out Of Time");
            //Bắt đầu bấm giờ cho RunOut
            StartTickRunOut();

            //Nếu vẫn đang trong thgian RunOut thì xử lý blink blink cho tempHP
            if (Time.time - _tempHPRunOutEntryTime < _tempHPRunoutDuration)
                HandleTempHPRunOutState();
            else
            {
                //Hết thgian RunOut máu ảo -> xoá lượng máu ảo và Reset Data liên quan đến nó
                HandleExpireTempHP();
                ResetDataRelatedToTempHP();
            }
        }
    }

    private void StartTickRunOut()
    {
        if (!_hasTickRunOut)
        {
            _hasTickRunOut = true;
            _tempHPRunOutEntryTime = Time.time;
            _tempHPEachRunOutEntryTime = Time.time;
        }
    }

    private void HandleTempHPRunOutState()
    {
        //Check tới lần Blink tiếp theo chưa
        if (Time.time - _tempHPEachRunOutEntryTime >= _timeEachBlink)
        {
            //Dựa trên bool check để set sprite cho từng máu ảo
            if (_blinkLost)
            {
                for (int i = _currentHP; i < _currentHP + _tempHP; i++)
                    _HPs[i]._state = HP_STATE_LOST;
                _blinkLost = false;
            }
            else
            {
                for (int i = _currentHP; i < _currentHP + _tempHP; i++)
                    _HPs[i]._state = HP_STATE_TEMP;
                _blinkLost = true;
            }
            _tempHPEachRunOutEntryTime = Time.time;
        }
    }

    private void HandleExpireTempHP()
    {
        //Func này để duyệt và set giá trị các TempHP, cách duyệt:
        //Bắt đầu từ VỊ TRÍ sau VỊ TRÍ của currentHP, nếu tempHP != 0 <=> ĐK for dưới thoả mãn thì:
        //Nếu vị trí của i VƯỢT QUÁ vị trí của MaxHP thì disable nó đi
        //Nếu 0 thì gán lại state lost cho vị trí đó
        for (int i = _currentHP; i < _currentHP + _tempHP; i++)
        {
            if (i > MaxHP - 1)
                _uiHP[i].enabled = false;
            else
                _HPs[i]._state = HP_STATE_LOST;
        }
    }

    private void ResetDataRelatedToTempHP()
    {
        _tempHP = 0;
        _hasGotTempHP = false;
        _hasTickRunOut = false;
    }

    public void RestartHP()
    {
        Start();
    }

    public void IncreaseHP()
    {
        if (_hasIncreaseHP) return;

        _hasIncreaseHP = true;
        _playerSO.MaxHP = PLAYER_MAX_HP_LEVEL_2;
        Start();
        Debug.Log("Incr");
    }

    public void DecreaseHP()
    {
        _hasIncreaseHP = false;
        _playerSO.MaxHP = PLAYER_MAX_HP_LEVEL_1;
    }
}
