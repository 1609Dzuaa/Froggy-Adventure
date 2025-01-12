using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;
using static GameEnums;

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

    [HideInInspector] public int MaxHP;
    private int _currentHP;
    private int _tempHP;
    bool _hasTempHP;
    Tween _firstTweenTempHP, _secondTweenTempHP;

    #region BLINK EFFECT FOR TEMP_HP WHEN RUNNING OUT
    private float _tempHPEntryTime;
    private float _tempHPRunOutEntryTime;
    private bool _blinkLost = true;
    #endregion

    public int CurrentHP { get { return _currentHP; } set { _currentHP = value; } }

    public int TempHP { get => _tempHP; set => _tempHP = value; }

    protected override void Awake()
    {
        base.Awake();
        InitUIHP();
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnAidForPlayer, AidForPlayer);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnStartCountTempHP, StartCountTempHP);
        EventsManager.Instance.SubcribeToAnEvent(EEvents.OnChangeHP, HandleChangeHP);
    }

    private void AidForPlayer(object obj)
    {
        _currentHP = DEFAULT_AID_PLAYER_HP;
        _tempHP = DEFAULT_AID_PLAYER_TEMP_HP;
        _hasTempHP = true;
        //Debug.Log("Aid");
    }

    private void StartCountTempHP(object obj)
    {
        if (_hasTempHP)
        {
            _firstTweenTempHP = DOTween.To(() => _tempHPEntryTime, x => _tempHPEntryTime = x, _tempHPDuration, _tempHPDuration).OnComplete(() =>
            {
                _tempHPRunOutEntryTime = Time.time;
                _tempHPEntryTime = 0f;
                _secondTweenTempHP = DOTween.To(() => _tempHPEntryTime, x => _tempHPEntryTime = x, _tempHPRunoutDuration, _tempHPRunoutDuration).OnUpdate(() =>
                {
                    //blink
                    if (Time.time - _tempHPRunOutEntryTime >= _timeEachBlink)
                    {
                        _tempHPRunOutEntryTime = Time.time;
                        for (int i = _currentHP; i <= _currentHP + _tempHP; i++)
                            _uiHP[i].sprite = (_blinkLost) ? _lostHPSprite : _tempHPSprite;
                        _blinkLost = !_blinkLost;
                    }
                }).OnComplete(() =>
                {
                    //reset
                    for (int i = _currentHP; i <= _currentHP + _tempHP; i++)
                        _uiHP[i].sprite = _lostHPSprite;
                    _hasTempHP = false;
                    _tempHP = 0;
                    _tempHPEntryTime = _tempHPRunOutEntryTime = 0f;
                });
            });
        }
    }

    private void HandleChangeHP(object obj)
    {
        EHPStatus status = (EHPStatus)obj;
        switch (status)
        {
            case EHPStatus.AddOneHP:
                _uiHP[_currentHP].sprite = _normalHPSprite;
                _currentHP++;
                Mathf.Clamp(_currentHP, MIN_HP, MaxHP);
                break;

            case EHPStatus.MinusOneHP:
                _currentHP--;
                Mathf.Clamp(_currentHP, MIN_HP, MaxHP);
                _uiHP[_currentHP].sprite = _lostHPSprite;
                break;

            case EHPStatus.AddOneTempHP:
                _hasTempHP = true;
                _uiHP[_currentHP + _tempHP].sprite = _tempHPSprite;
                _tempHP++;
                Mathf.Clamp(_tempHP, MIN_HP, MaxHP);
                break;

            case EHPStatus.LooseAll: //ăn curse
                for (int i = 0; i <= _currentHP + _tempHP; i++)
                    _uiHP[i].sprite = _lostHPSprite;
                _firstTweenTempHP.Kill();
                _secondTweenTempHP.Kill();
                _currentHP = _tempHP = 0;
                break;
        }
        Debug.Log("HP change, current: " + _currentHP);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnAidForPlayer, AidForPlayer);
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnStartCountTempHP, StartCountTempHP);
        EventsManager.Instance.UnsubscribeToAnEvent(EEvents.OnChangeHP, HandleChangeHP);
    }

    private void InitUIHP()
    {
        for (int i = 0; i < PLAYER_MAX_HP; i++)
        {
            _uiHP[i].enabled = i < MaxHP;
            _uiHP[i].sprite = (i < _currentHP) ? _normalHPSprite : _lostHPSprite;
        }
    }
}
