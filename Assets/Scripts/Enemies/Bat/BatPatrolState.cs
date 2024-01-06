using UnityEngine;

public class BatPatrolState : MEnemiesPatrolState
{
    private int _randomDirection;
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        //Thằng dưới gọi là Manager chung của các enemies
        //Nó dùng để Reference thằng Manager class, truy cập và lấy data từ class đó
        _mEnemiesManager = (MEnemiesManager)charactersManager; //recheck here why it got null
        //Null vì ở dưới có gọi Manager chung ở đoạn rest
        //Nếu 0 muốn phải convert thằng manager chung thì cân nhắc thêm tham số Manager cho function chung
        _batManager = (BatManager)charactersManager;
        _entryTime = Time.time;
        _batManager.Animator.SetInteger("state", (int)GameEnums.EBatState.patrol);
        _randomDirection = Random.Range(0, 4); //random bay 4 hướng ?, dùng switch case
        //Bỏ random kiểu Int switch sang random float đc 1 lượng offset đem * với velo.y
        Debug.Log("Pt");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (CheckIfCanRest()) //cân nhắc truyền tham số là dạng Manager cho hàm
        {
            _batManager.BatIdleState.HasPatrol = true; //Cho phép bay về ngủ 1 khi đã patrol xong
            _batManager.ChangeState(_batManager.BatIdleState);
        }
        else if (CheckIfCanAttack()) //recheck here
            _batManager.ChangeState(_batManager.BatAttackState); //Bat thì chắc change luôn 0 cần delay
        //_batManager.Invoke("AllowAttackPlayer", _batManager.GetAttackDelay());
        else if (CheckIfNeedRetreat())
            _batManager.ChangeState(_batManager.BatRetreatState);
        else if (!_hasChangedState)
            HandleFlyPatrol();

        //Debug.Log("Allow back, hasPT: " + _allowBackToSleepPos + _batManager.BatIdleState.HasPatrol);
    }

    protected override bool CheckIfCanAttack()
    {
        return _batManager.HasDetectedPlayer;
    }

    private void HandleFlyPatrol()
    {
        switch (_randomDirection)
        {
            case 0:
                if (_batManager.GetIsFacingRight())
                    _batManager.FlipLeft();
                _batManager.GetRigidbody2D().velocity = new Vector2(-1f, 1f) * _batManager.GetPatrolSpeed();
                //_batManager.transform.position += new Vector3(-1 * _batManager.GetPatrolSpeed().x, _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
            case 1:
                if (!_batManager.GetIsFacingRight())
                    _batManager.FlipRight();
                _batManager.GetRigidbody2D().velocity = _batManager.GetPatrolSpeed();
                //_batManager.transform.position += new Vector3(_batManager.GetPatrolSpeed().x, _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
            case 2:
                if (_batManager.GetIsFacingRight())
                    _batManager.FlipLeft();
                _batManager.GetRigidbody2D().velocity = new Vector2(-1f, -1f) * _batManager.GetPatrolSpeed();
                //_batManager.transform.position += new Vector3(-1 * _batManager.GetPatrolSpeed().x, -1 * _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
            case 3:
                if (!_batManager.GetIsFacingRight())
                    _batManager.FlipRight();
                _batManager.GetRigidbody2D().velocity = new Vector2(1f, -1f) * _batManager.GetPatrolSpeed();
                //_batManager.transform.position += new Vector3(_batManager.GetPatrolSpeed().x, -1 * _batManager.GetPatrolSpeed().y, 0f) * Time.deltaTime;
                break;
        }

        //Xử lý việc bay patrol (bao gồm check hướng đã random để patrol
        //và di chuyển cũng như flip sprite cho hợp lý)
    }

    public override void FixedUpdate() { }

    private bool CheckIfNeedRetreat()
    {
        //Thêm đk check này vì
        //Vẫn có TH:
        //bat mới dậy và chase player tới khi player tàng hình
        //và về Idle r Patrol tiếp nhưng lúc này Patrol lại vượt quá safe_range của nó 

        if (_batManager.transform.position.x >= _batManager.BoundaryRight.position.x)
        {
            _batManager.FlipLeft();
            return true;
        }
        else if (_batManager.transform.position.x <= _batManager.BoundaryLeft.position.x)
        {
            _batManager.FlipRight();
            return true;
        }
        return false;
    }
}
