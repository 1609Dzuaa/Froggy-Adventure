﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TMP = TextMeshPro

public class Dialog : MonoBehaviour
{
    //Chỉnh Font Style với TextMeshPro:
    //https://www.youtube.com/watch?v=A33HoKdOhTQ
    //Cân nhắc các font khác:
    //Pixel Operator, Munro, Roboto Mono
    //Font hiện tại: PressStart2P khá lỏ với tiếng Việt

    [SerializeField] private GameObject _window; //Hộp chứa Thoại
    [SerializeField] private GameObject _indicator; //Chỉ dẫn ("Press ... to start conversation")
    [SerializeField] private TMP_Text _dialogText; //Ô Thoại
    [SerializeField] private TMP_Text _indicatorText; //Ô Chỉ Dẫn

    //Mặc định các NPC trong game sẽ chỉ có 2 cuộc hội thoại
    [SerializeField] private List<string> _dialog; //Thoại 1
    [SerializeField] private List<string> _dialog2; //Thoại 2

    [SerializeField] private List<string> _indicatorString;
    [SerializeField] private float _writingSpeed; //Tốc độ viết Thoại
    [SerializeField] private bool _dontNeedStartIndicatorTextAgain; //tick vào nếu sau khi hội thoại xong 0 muốn indicator trở về text ban đầu

    private int _rowIndex; //Chỉ số hàng
    private int _charIndex; //Chỉ số char trong string
    private bool _started; //Biến đánh dấu đã bắt đầu Thoại
    private bool _isWaiting; //Biến đánh dấu chờ Player tương tác
    private bool _cantGetNextIndicatorText;
    private bool _isFinishedFirstConversation;

    //Biến check nếu bắt chuyện theo cách bị động (0 nhấn T mà làm gì đó NPC trước)
    //VD: với Slime thì Player có thể chọn nhấn T hoặc Jump lên đầu nó để Start Conversation(SC)
    private bool _startConversationPassive;

    public bool Started { get { return _started; } }

    public bool IsWaiting { get { return _isWaiting; } }

    public bool CantGetNextIndicatorText { set { _cantGetNextIndicatorText = value; } }

    public bool StartConversationPassive { set { _startConversationPassive = value; } }

    public bool IsFinishedFirstConversation { get => _isFinishedFirstConversation; }

    private void Awake()
    {
        //Khởi động thì tắt Hộp lẫn Indicator
        ToggleWindow(false);
        ToggleIndicator(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Render chỉ dẫn đầu tiên
        if (_indicatorText)
            _indicatorText.text = _indicatorString[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (_indicatorText)
            _indicatorText.transform.eulerAngles = Vector3.zero;

        //Chỉ khi bắt đầu Thoại thì mới Update
        if (!_started) 
            return;

        //0 cho Thoại quay lung tung
        _dialogText.transform.eulerAngles = Vector3.zero;

        //Chờ Player tương tác (Nhấn Space nếu SC chủ động hoặc T nếu SC bị động)
        //Mục đích của SC bị động là nhấn T sẽ lấy string trò chuyện tiếp theo luôn
        if (_isWaiting && Input.GetKeyDown(KeyCode.Space) && !_startConversationPassive
            || _isWaiting && Input.GetKeyDown(KeyCode.T) && _startConversationPassive)
        {
            //Restart cho SC bị động lần sau
            if (_startConversationPassive)
                _startConversationPassive = false;

            _isWaiting = false; //0 phải đợi nữa

            if(!_isFinishedFirstConversation)
            {
                _rowIndex++; //Xuống hàng kế tiếp

                //Check hàng hiện tại CHƯA vượt quá hàng thực tế thì
                //bắt đầu Thoại hàng kế tiếp
                //Còn 0 thì kết thúc Thoại
                if (_rowIndex < _dialog.Count)
                    GetDialog(_rowIndex);
                else
                {
                    UpdateIndicatorText();
                    EndDialog();
                }
            }
            else
            {
                _rowIndex++; //Xuống hàng kế tiếp
                if (_rowIndex < _dialog2.Count)
                    GetDialog(_rowIndex);
                else
                {
                    UpdateIndicatorText();
                    EndDialog();
                }
            }
        }
    }

    private void UpdateIndicatorText()
    {
        if (!_dontNeedStartIndicatorTextAgain)
            _indicatorText.text = _indicatorString[0];
        else
            _indicatorText.text = _indicatorString[1];
    }

    public void ToggleWindow(bool show)
    {
        _window.SetActive(show);
        //Dùng để bật/tắt Hộp
    }

    public void ToggleIndicator(bool show)
    {
        if (_indicator)
            _indicator.SetActive(show);
        //Dùng để bật/tắt Indicator
    }

    public void StartDialog(int index)
    {
        if(_started) 
            return;

        _started = true;
        ToggleWindow(true);
        ToggleIndicator(true);
        GetDialog(index);
        //Bắt đầu Thoại
        //Bật Hộp và Indicator cũng như bắt đầu Thoại đầu tiên
    }

    public void EndDialog()
    {
        //Trả lại chỉ dẫn ban đầu khi end Thoại
        /*if (!_isBackToNextIndicatorText)
            _indicatorText.text = _indicatorString[0];
        else 
            _indicatorText.text = _indicatorString[1];*/

        _started = false;
        StopAllCoroutines();
        ToggleWindow(false);

        //Kết thúc cuộc hội thoại đầu tiên
        //Cho phép cuộc hội thoại thứ 2 (Nếu có)
        if (!_isFinishedFirstConversation)
            _isFinishedFirstConversation = true;
        Debug.Log("End: " + _isFinishedFirstConversation);
        //Kết thúc Thoại
        //Dừng mọi Coroutines và tắt Hộp
    }

    private void GetDialog(int i)
    {
        _rowIndex = i; //Chỉ định hàng i
        _charIndex = 0; //Bắt đầu ở chỉ số 0
        _dialogText.text = string.Empty; //Làm "sạch" Thoại hiện tại
        StartCoroutine(Writing());
        //StartCoroutine tương đồng với Invoke,
        //có thể cải thiện hiệu năng đáng kể, hàm đc gọi trong nó phải là kiểu IEnumerator
        //Ở đây gọi thằng Writing

        //NOTE:
        //Start Coroutine giúp thực hiện các tác vụ có thể chạy
        //và dừng lại tại các điểm cụ thể.

        //Lấy Thoại ở hàng i và Render nó lên màn hình
    }

    IEnumerator Writing()
    {
        //Cú pháp dưới sẽ làm hàm Writing đợi sau _writingSpeed
        yield return new WaitForSeconds(_writingSpeed);

        //Gán chuỗi hộp thoại hiện tại vào biến currentDialog
        string currentDialog;
        if (!_isFinishedFirstConversation)
            currentDialog = _dialog[_rowIndex];
        else 
            currentDialog = _dialog2[_rowIndex];

        //Render Thoại(Render từng chữ) lên màn hình
        _dialogText.text += currentDialog[_charIndex];

        //Tăng chỉ số của char
        ++_charIndex;

        //Check chỉ số char mà CHƯA vượt quá mảng Thoại hàng hiện tại
        //thì đợi trong _writingSpeed (s) để vẽ chữ cái tiếp
        //Còn 0 thì đánh dấu đợi Player tương tác

        //Là đệ quy với ĐK dừng ở dưới:
        if (_charIndex < currentDialog.Length)
            StartCoroutine(Writing());
        else
        {
            if (_indicatorText)
            {
                _indicatorText.text = string.Empty;
                if (_rowIndex >= 0 && _indicatorString.Count > 1 && !_cantGetNextIndicatorText)
                    _indicatorText.text = _indicatorString[1]; //Render chỉ dẫn thứ 2
                else
                    _indicatorText.text = _indicatorString[0];
            }
            ToggleIndicator(true);
            _isWaiting = true;
        }
    }
}
