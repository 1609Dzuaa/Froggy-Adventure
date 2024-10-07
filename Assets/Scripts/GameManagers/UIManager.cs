using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameEnums;
using static GameConstants;
using DG.Tweening;

[Serializable]
public struct PopupUI
{
    public EPopup ID;
    public Canvas UICanvas;
}

[Serializable]
public struct ToggleButton
{
    public EToggleButton Name;
    public GameObject ButtonOn;
    public GameObject ButtonOff;
}

public class UIManager : BaseSingleton<UIManager>
{
    [SerializeField] TextMeshProUGUI _txtFPS;
    [SerializeField] List<PopupUI> _listPopupUI;
    [SerializeField] Canvas _lockUICanvas;
    [SerializeField] List<ToggleButton> _listToggleBtn;
    [SerializeField] Canvas[] _arrMenuUICanvas;
    [SerializeField] Canvas _HUDCanvas;

    [Header("Times")]
    [SerializeField] float _delayTrans1;
    [SerializeField] float _delayTrans2;

    [SerializeField, Tooltip("Thời gian tween chuyển scene")] float _transDuration;

    [Header("Animation While Switch Scene")]
    [SerializeField] TweenListGameObjects _buttonContainer;
    [SerializeField] Transform _imageSceneTrans;
    [SerializeField] PopupResult _popupResult;
    [SerializeField] PopupNotification _popupNotification;
    [SerializeField] HUDController _hudControl;

    #region Internal Attributes
    Dictionary<EPopup, Canvas> _dictPopupUI = new();
    Dictionary<EToggleButton, ToggleButton> _dictToggleBtn = new();
    Stack<Canvas> _stackPopupCanvas = new();
    int _popupSortOrder = 1;
    //từ điển lưu các gameobject UI cần Popup, dễ mở rộng hơn
    //thay thế mấy đoạn popup...canvas... = popup(tham số)
    #endregion

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(hell());
    }

    private IEnumerator hell()
    {
        yield return null;
        if (gameObject)
            Debug.Log("UIManager awake");
    }

    private void Start()
    {
        FillInDictionary();
        ToggleInGameCanvas(false);
        ResetAllPopupSortOrder();
        _buttonContainer.Tween(true);
    }

    private void FillInDictionary()
    {
        foreach (var item in _listPopupUI)
        {
            item.UICanvas.gameObject.SetActive(false);
            if (!_dictPopupUI.ContainsKey(item.ID))
                _dictPopupUI.Add(item.ID, item.UICanvas);
        }

        foreach (var item in _listToggleBtn)
        {
            if (!_dictToggleBtn.ContainsKey(item.Name))
                _dictToggleBtn.Add(item.Name, item);
        }
    }

    public void TogglePopup(EPopup id, bool isOn)
    {
        if (isOn)
        {
            _stackPopupCanvas.Push(_dictPopupUI[id]);

            //popup mới đc thêm vào stack sẽ có sortOrder
            //cao nhất và sau nó là LockUI
            _popupSortOrder++;
            _dictPopupUI[id].sortingOrder = _popupSortOrder;
            _lockUICanvas.sortingOrder = _popupSortOrder - 1;

            //nếu lượng phần tử stack nhiều hơn 1 thì:
            //cập nhật sortOrder của phần tử kề top stack
            //sao cho nó chỉ đứng sau sortOrder của LockUI và
            //phần tử top (sortOrder cao nhất)
            if (_stackPopupCanvas.Count > 1)
                _stackPopupCanvas.ToArray()[1].sortingOrder = _popupSortOrder - 2;

            _dictPopupUI[id].gameObject.SetActive(true);
            _lockUICanvas.gameObject.SetActive(true);
        }
        else
        {
            //nếu tắt đi 1 popup thì:
            //Lôi top ra khỏi stack, giảm sortOrder đi 1 đơn vị
            //(vì top đã bị lấy ra)
            //nếu trong stack có phần tử popup khác thì:
            //gắn sortOrder của popup đó lại = sortOrder
            //cũng như sortOrder của LockUI = sortOrder - 1

            _stackPopupCanvas.Pop().gameObject.SetActive(false);
            if (_stackPopupCanvas.Count > 0)
            {
                _popupSortOrder--;
                _stackPopupCanvas.Peek().sortingOrder = _popupSortOrder;
                _lockUICanvas.sortingOrder = _popupSortOrder - 1;
            }
            else
            {
                _popupSortOrder = 1;
                ResetAllPopupSortOrder();
            }
            _lockUICanvas.gameObject.SetActive((_stackPopupCanvas.Count == 0) ? false : true);
        }

        //Debug.Log("Popup: " + id + ", isOn: " + isOn + ", S-order: " + _popupSortOrder);
    }

    private void ResetAllPopupSortOrder()
    {
        foreach (var item in _dictPopupUI)
            item.Value.sortingOrder = _popupSortOrder;
    }

    public void ToggleInGameCanvas(bool isOn)
    {
        _HUDCanvas.gameObject.SetActive(isOn);
        //Canvas này chỉ bật khi đang trong Gameplay
    }

    public void ToggleButtonOnClick(EToggleButton btn, bool isBtnOn)
    {
        _dictToggleBtn[btn].ButtonOn.SetActive(!isBtnOn);
        _dictToggleBtn[btn].ButtonOff.SetActive(isBtnOn);
    }

    public void ToggleMenuUIsCanvas(bool isOn)
    {
        foreach (var canvas in _arrMenuUICanvas)
            canvas.gameObject.SetActive(isOn);
    }

    public void HandleDisplayMenuUI()
    {
        _buttonContainer.Tween(true);
    }

    /// <summary>
    /// Chuyển scene = hàm này, 0 phải hàm bên GameManager
    /// </summary>
    /// <param name="indexLevel">Chú ý thằng này phải khớp với Build Index ngoài Editor</param>
    /// <param name="needReset">Có cần bắn event reset vài thứ khi chuyển scene không</param>
    /// <param name="isReplay">Nếu chơi lại thì 0 cần phải start new countdown</param>
    public void AnimateAndTransitionScene(int indexLevel, bool needReset = false, bool isReplay = false)
    {
        if (SceneManager.GetActiveScene().buildIndex == GAME_MENU)
        {
            //cần tween cụm button
            _buttonContainer.Tween(false);
            StartCoroutine(HandleTransitionAndSwitchScene(indexLevel, _delayTrans1));
        }
        else
        {
            //scene lúc này 0 phải là menu nên cần check có popupresult ko
            //để gọi hàm tween nó
            //nếu 0 phải thì chỉ còn TH out về MainMenu trong Gameplay => gọi tween Noti
            if (_dictPopupUI[EPopup.Result].gameObject.activeInHierarchy)
            {
                _popupResult.OnClose();
                StartCoroutine(HandleTransitionAndSwitchScene(indexLevel, _delayTrans1, needReset, isReplay));
            }
            else
            {
                _popupNotification.OnClose();
                StartCoroutine(HandleTransitionAndSwitchScene(indexLevel, _delayTrans2, needReset, isReplay));
            }
        }
    }

    private IEnumerator HandleTransitionAndSwitchScene(int indexLevel, float waitTime, bool needReset = false, bool isReplay = false)
    {
        yield return new WaitForSeconds(waitTime);

        _imageSceneTrans.DOLocalMoveX(0f, _transDuration).OnComplete(() =>
        {
            ToggleMenuUIsCanvas((indexLevel != GAME_MENU) ? false : true);
            ToggleInGameCanvas((indexLevel != GAME_MENU) ? true : false);
            GameManager.Instance.SwitchScene(indexLevel);
            if (needReset)
                EventsManager.Instance.NotifyObservers(EEvents.OnResetLevel);
            _imageSceneTrans.DOLocalMoveX(-5000f, _transDuration).OnComplete(() =>
            {
                if (SceneManager.GetActiveScene().buildIndex == GAME_MENU)
                    HandleDisplayMenuUI();
                else if (!isReplay)
                {
                    _hudControl.Countdown(); //0 phải replay thì mới count
                }
                List<Skills> skills = ToggleAbilityItemHelper.GetListActivatedSkills();
                EventsManager.Instance.NotifyObservers(EEvents.OnValidatePlayerBuffs, skills);
                _imageSceneTrans.position = new(6652f, _imageSceneTrans.position.y);
            });
        });
    }

    private void OnEnable()
    {
    }

    private void OnDestroy()
    {
        //Debug.Log("UI Manager OnDestroy, this, frame: " + this + Time.frameCount);
    }

    private void Update()
    {       
        ShowFPS();
    }

    private void ShowFPS()
    {
        double fps = 1 / Time.deltaTime;
        fps = Math.Round(fps, 2);
        _txtFPS.text = fps.ToString();
    }
}
