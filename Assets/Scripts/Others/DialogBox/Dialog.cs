using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TMP = TextMeshPro
using System.Net.NetworkInformation;

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
    [SerializeField] private List<string> _dialog; //Thoại
    [SerializeField] private float _writingSpeed; //Tốc độ viết Thoại

    private int _rowIndex; //Chỉ số hàng
    private int _charIndex; //Chỉ số char trong string
    private bool _started; //Biến đánh dấu đã bắt đầu Thoại
    private bool _isWait; //Biến đánh dấu chờ Player tương tác

    private void Awake()
    {
        //Khởi động thì tắt Hộp lẫn Indicator
        ToggleWindow(false);
        ToggleIndicator(false);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Chỉ khi bắt đầu Thoại thì mới Update
        if (!_started) 
            return;

        //Cố định, đéo cho quay khi parent quay :)
        //0 có 2 dòng dưới thì khi quay phải text sẽ bị ngược
        if (_dialogText.transform.eulerAngles.y > 0f)
            _dialogText.transform.eulerAngles = Vector3.zero;

        //Chờ Player tương tác (Nhấn Space)
        if (_isWait && Input.GetKeyDown(KeyCode.Space))
        {
            _isWait = false; //0 phải đợi nữa
            _rowIndex++; //Xuống hàng kế tiếp
            
            //Check hàng hiện tại chưa vượt quá hàng thực tế thì bắt đầu Thoại hàng kế tiếp
            //Còn 0 thì kết thúc Thoại
            if (_rowIndex < _dialog.Count)
                GetDialog(_rowIndex);
            else
                EndDialog();
        }
    }

    public void ToggleWindow(bool show)
    {
        _window.SetActive(show);
        //Dùng để bật/tắt Hộp
    }

    public void ToggleIndicator(bool show)
    {
        _indicator.SetActive(show);
        //Dùng để bật/tắt Indicator
    }

    public void StartDialog()
    {
        if(_started) 
            return;
        _started = true;
        ToggleWindow(true);
        ToggleIndicator(true);
        GetDialog(0);
        //Bắt đầu Thoại
        //Bật Hộp và Indicator cũng như bắt đầu Thoại đầu tiên
    }

    public void EndDialog()
    {
        _started = false;
        StopAllCoroutines();
        ToggleWindow(false);
        //Kết thúc Thoại
        //Dừng mọi Coroutines và tắt Hộp
    }

    private void GetDialog(int i)
    {
        _rowIndex = i; //Chỉ định hàng i
        _charIndex = 0; //Bắt đầu ở chỉ số 0
        _dialogText.text = string.Empty; //Làm "sạch" Thoại hiện tại
        StartCoroutine(Writing()); //StartCoroutine tương đồng với Invoke,
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
        string currentDialog = _dialog[_rowIndex];

        //Render Thoại(Render từng chữ) lên màn hình
        _dialogText.text += currentDialog[_charIndex];

        //Tăng chỉ số của char
        _charIndex++;

        //Đợi trong _writingSpeed (s)
        yield return new WaitForSeconds(_writingSpeed);

        //Check chỉ số char mà CHƯA vượt quá mảng Thoại hàng hiện tại
        //thì đợi trong _writingSpeed (s) để vẽ chữ cái tiếp
        //Còn 0 thì đánh dấu đợi Player tương tác
        if (_charIndex < currentDialog.Length)
        {
            yield return new WaitForSeconds(_writingSpeed);
            StartCoroutine(Writing());
        }
        else
            _isWait = true;
    }
}
