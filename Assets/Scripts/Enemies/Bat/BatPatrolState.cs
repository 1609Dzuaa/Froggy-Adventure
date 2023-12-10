using UnityEngine;

public class BatPatrolState : MEnemiesPatrolState
{
    private bool _allowBackToSleepPos;
    private bool _hasFlip;
    private int _randomDirection;
    private BatManager _batManager;

    public bool AllowBackToSleepPos { set { _allowBackToSleepPos = value; } }

    public override void EnterState(CharactersManager charactersManager)
    {
        //Thằng dưới gọi là Manager chung của các enemies
        //Nó dùng để Reference thằng Manager class, truy cập và lấy data từ class đó
        _mEnemiesManager = (MEnemiesManager)charactersManager; //recheck here why it got null
        //Null vì ở dưới có gọi Manager chung ở đoạn rest
        //Nếu 0 muốn phải convert thằng manager chung thì cân nhắc thêm tham số Manager cho function chung
        _batManager = (BatManager)charactersManager;
        _entryTime = Time.time;
        _batManager.Animator.SetInteger("state", (int)EnumState.EBatState.patrol);
        _randomDirection = Random.Range(0, 4); //random bay 4 hướng ?, dùng switch case
        //Bỏ random kiểu Int switch sang random float đc 1 lượng offset đem * với velo.y
    }

    public override void ExitState()
    {
        base.ExitState();
        _hasFlip = false;
    }

    public override void Update()
    {
        if (CheckIfCanRest() && !_allowBackToSleepPos) //cân nhắc truyền tham số là dạng Manager cho hàm
        {
            _batManager.ChangeState(_batManager.BatIdleState);
            _allowBackToSleepPos = true;
            //Debug.Log("Has PT: " + _batManager.BatIdleState.HasPatrol);
        }
        else if (CheckIfCanAttack()) //recheck here
            _batManager.Invoke("AllowAttackPlayer", _batManager.GetAttackDelay());
        else if (_allowBackToSleepPos && !_hasChangedState)
        {
            HandleFlyBackToSleepPos();
            //Debug.Log("here");
        }
        else if (!_hasChangedState)
            HandleFlyPatrol();

        //Debug.Log("Allow back, hasPT: " + _allowBackToSleepPos + _batManager.BatIdleState.HasPatrol);
    }

    private void HandleFlyPatrol()
    {
        switch (_randomDirection)
        {
            case 0:
                if (_batManager.GetIsFacingRight())
                    _batManager.FlipLeft();
                _batManager.transform.position += new Vector3(-1 * _batManager.GetPatrolSpeed().x, _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
            case 1:
                if (!_batManager.GetIsFacingRight())
                    _batManager.FlipRight();
                _batManager.transform.position += new Vector3(_batManager.GetPatrolSpeed().x, _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
            case 2:
                if (_batManager.GetIsFacingRight())
                    _batManager.FlipLeft();
                _batManager.transform.position += new Vector3(-1 * _batManager.GetPatrolSpeed().x, -1 * _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
            case 3:
                if (!_batManager.GetIsFacingRight())
                    _batManager.FlipRight();
                _batManager.transform.position += new Vector3(_batManager.GetPatrolSpeed().x, -1 * _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
        }

        //Xử lý việc bay patrol (bao gồm check hướng đã random để patrol
        //và di chuyển cũng như flip sprite cho hợp lý)
    }

    public override void FixedUpdate() { }

    private void HandleFlyBackToSleepPos()
    {
        if (!_hasFlip)
            HandleFlipSprite();

        //_maxDistance = Mathf.Sqrt(_batManager.GetPatrolSpeed() * _batManager.GetPatrolSpeed().x + _batManager.GetPatrolSpeed().y * _batManager.GetPatrolSpeed().y);
        _batManager.transform.position = Vector2.MoveTowards(_batManager.transform.position, _batManager.SleepPos.position, _batManager.GetPatrolSpeed().x * Time.deltaTime);
        if(CheckCanCeilIn())
            _batManager.ChangeState(_batManager.BatCeilInState);
        //Xử lý việc bay về chỗ ngủ và chuyển trạng thái Ceil In
    }

    private void HandleFlipSprite()
    {
        _hasFlip = true;

        if (_batManager.transform.position.x > _batManager.SleepPos.position.x)
            _batManager.FlipLeft();
        else if (_batManager.transform.position.x < _batManager.SleepPos.position.x)
            _batManager.FlipRight();
        //Xử lý lật sprite trước khi bay về chỗ ngủ
    }

    private bool CheckCanCeilIn()
    {
        if (Vector2.Distance(_batManager.transform.position, _batManager.SleepPos.position) < 0.01f)
        {
            //_allowBackToSleepPos = false;
            return true;
        }
        return false;
    }
}
