using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Thêm cơ chế Move Cam cơ bản, trước mắt tạm ổn
    //NHƯNG có thể vẫn còn bug nên cần xem lại sau

    [Header("Character References")]
    [SerializeField] private Transform _playerRef;

    [Header("TriggerZone Field")]
    [SerializeField] private Transform _triggerZone;
    [SerializeField] private Vector3 _distanceMove;

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

        this.transform.position = new Vector3(_playerRef.position.x, _playerRef.position.y, _playerRef.position.z - 10);
    }

    private void Update()
    {
        if (_hasTriggered)
            MovingToNewPosition(_posNeedToMove);
        else if (_hasLeavedTriggerZone && _mustMoveBack)
            MovingBackToPlayer(_playerRef);
        else
            MovingALongPlayer();
    }

    private void MovingALongPlayer()
    {
        this.transform.position = new Vector3(_playerRef.position.x, _playerRef.position.y, _playerRef.position.z - 10);
    }

    private void MovingToNewPosition(Transform newPos)
    {
        if (Mathf.Abs(transform.position.x - newPos.position.x) > GameConstants.CAMERASAFERANGE)
            transform.position += new Vector3(_distanceMove.x, _distanceMove.y, _distanceMove.z);
        _mustMoveBack = true;
        //Hàm này để Move Cam tới vị trí mới và đánh dấu cần phải move back về player
    }

    private void MovingBackToPlayer(Transform player)
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > GameConstants.CAMERASAFERANGE)
            transform.position -= new Vector3(_distanceMove.x, _distanceMove.y, _distanceMove.z);
        else
            _mustMoveBack = false;
        //Hàm này để Move Cam từ từ về Player sau khi rời TriggerZone và đánh dấu (nếu đủ đk) 0 cần move back
    }
}
