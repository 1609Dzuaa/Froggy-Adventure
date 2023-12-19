using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Move Cam về phía trước và thu Cam về Player đã mượt hơn
    //Cân nhắc cơ chế đi quá nửa màn hình r hẵng move Cam
    //Hoặc move Cam theo độ Smooth

    [Header("Character References")]
    [SerializeField] private Transform _targetToFollow;

    [Header("TriggerZone Field")]
    [SerializeField] private Transform _triggerZone;
    [SerializeField] private float _distanceMove;

    private bool _hasTriggered;
    private bool _hasLeavedTriggerZone;
    private bool _mustMoveBack;
    private Transform _posNeedToMove;

    private static CameraController _instance;

    public bool HasTriggered { get { return _hasTriggered; } set { _hasTriggered = value; } }

    public bool HasLeavedTriggerZone { set { _hasLeavedTriggerZone = value; } }

    public Transform PosNeedToMove { set {  _posNeedToMove = value; } }

    public static CameraController GetInstance() { return _instance; }

    private void Start()
    {
        //One and Only
        if (_instance == null)
            _instance = this;

        this.transform.position = new Vector3(_targetToFollow.position.x, _targetToFollow.position.y, _targetToFollow.position.z - 10);
    }

    private void LateUpdate()
    {
        if (_hasTriggered)
            FollowNewTarget(_posNeedToMove);
        else if (_hasLeavedTriggerZone && _mustMoveBack)
            FollowBackToPlayer(_targetToFollow);
        else
            FollowALongPlayer();
    }

    private void FollowALongPlayer()
    {
        this.transform.position = new Vector3(_targetToFollow.position.x, _targetToFollow.position.y, _targetToFollow.position.z - 10);
    }

    private void FollowNewTarget(Transform newTarget)
    {
        if (Vector2.Distance(transform.position, newTarget.position) > GameConstants.CAMERASAFERANGE)
        {
            Vector3 _newPos = new Vector3(newTarget.position.x, newTarget.position.y, newTarget.position.z - 10);
            transform.position = Vector3.Lerp(transform.position, _newPos, _distanceMove * Time.deltaTime);

            //Lerp: Nhận vào 3 tham số là 2 điểm a, b và tỉ lệ t
            //Nó sẽ trả về giá trị nằm giữa 2 điểm a, b theo ct:
            //Value = a*x - (b+a)*x*t

            //Dùng Nội Suy Tuyến Tính (LERP) để di chuyển cam theo target cho mượt
            //Vì dùng DeltaTime nên giá trị mà Lerp trả về sẽ 0 bao giờ = giá trị thực tế
            //nên mình lấy ~ (Dùng GameConstants.CAMERASAFERANGE)
            //https://www.youtube.com/watch?v=MyVY-y_jK1I 4:10
        }
        _mustMoveBack = true;
        //Hàm này để Move Cam tới vị trí mới và đánh dấu cần phải move back về player
    }

    private void FollowBackToPlayer(Transform player)
    {
        if (Vector2.Distance(transform.position, player.position) > GameConstants.CAMERASAFERANGE)
        {
            Vector3 newPos = new Vector3(player.position.x, player.position.y, player.position.z - 10);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * _distanceMove);
        }
        else
            _mustMoveBack = false;

        //Hàm này để Move Cam từ từ về Player sau khi rời TriggerZone và đánh dấu (nếu đủ đk) 0 cần move back
    }
}
