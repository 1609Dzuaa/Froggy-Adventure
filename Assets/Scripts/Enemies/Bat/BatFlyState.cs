using UnityEngine;

public class BatFlyState : BatBaseState
{
    private float distance; //Tính khcach với player để chase
    private float entryTime; //Tính thgian bay để rest (idle)
    private int randomDirection; //Random: 0 - Left UP; 1 - Right UP; 2 - Left DOWN; 3 - Right DOWN
    private bool hasFlip = false; //Trước khi bay về chỗ ngủ thì lật sprite
    private bool hasFlyPatrol = false; //Nếu đã Fly patrol r thì cho phép về ngủ
    private float maxDistance;

    public bool GetHasFlyPatrol() { return this.hasFlyPatrol; }

    public override void EnterState(BatStateManager batStateManager)
    {
        base.EnterState(batStateManager);
        _batStateManager.GetAnimator().SetInteger("state", (int)EnumState.EBatState.fly);
        entryTime = Time.time;
        randomDirection = Random.Range(0, 4); //random bay 4 hướng ?, dùng switch case
        Debug.Log("Fly");
    }

    public override void ExitState() 
    { 
        hasFlip = false;
    }

    public override void UpdateState()
    {
        //Tính khcach từ bat đến player để check ở dưới
        distance = Vector2.Distance(_batStateManager.transform.position, _batStateManager.GetPlayer().position);
        if (_batStateManager.GetAttackRange() >= distance)
            AllowToChase();
        else if (Time.time - entryTime >= _batStateManager.GetRestTime() && !hasFlyPatrol)
            AllowToRest();
        else if (!hasFlyPatrol)
            HandleFlyPatrol();
        else //Bay về ngủ
            HandleFlyBackToSleepPos();
    }

    private void AllowToChase()
    {
        //Trước khi chuyển sang chase thì đánh dấu ch patrol
        //cho lần tới enter state này 
        hasFlyPatrol = false; 
        _batStateManager.ChangState(_batStateManager.batChaseState);
    }

    private void AllowToRest()
    {
        _batStateManager.ChangState(_batStateManager.batIdleState);
        hasFlyPatrol = true;
        //Check nếu đã hoàn thành patrol => cho phép về ngủ
    }

    private void HandleFlyPatrol()
    {
        switch (randomDirection)
        {
            case 0:
                if (_batStateManager.GetIsFacingRight())
                    _batStateManager.FlipLeft();
                _batStateManager.transform.position += new Vector3(-1 * _batStateManager.GetFlySpeed().x, _batStateManager.GetFlySpeed().y, 0f) * Time.deltaTime;
                break;
            case 1:
                if (!_batStateManager.GetIsFacingRight())
                    _batStateManager.FlipRight();
                _batStateManager.transform.position += new Vector3(_batStateManager.GetFlySpeed().x, _batStateManager.GetFlySpeed().y, 0f) * Time.deltaTime;
                break;
            case 2:
                if (_batStateManager.GetIsFacingRight())
                    _batStateManager.FlipLeft();
                _batStateManager.transform.position += new Vector3(-1 * _batStateManager.GetFlySpeed().x, -1 * _batStateManager.GetFlySpeed().y, 0f) * Time.deltaTime;
                break;
            case 3:
                if (!_batStateManager.GetIsFacingRight())
                    _batStateManager.FlipRight();
                _batStateManager.transform.position += new Vector3(_batStateManager.GetFlySpeed().x, -1 * _batStateManager.GetFlySpeed().y, 0f) * Time.deltaTime;
                break;
        }

        //Xử lý việc bay patrol (bao gồm check hướng đã random để patrol
        //và di chuyển cũng như flip sprite cho hợp lý)
    }

    private void HandleFlyBackToSleepPos()
    {
        if (!hasFlip)
            HandleFlipSprite();

        maxDistance = Mathf.Sqrt(_batStateManager.GetFlySpeed().x * _batStateManager.GetFlySpeed().x + _batStateManager.GetFlySpeed().y * _batStateManager.GetFlySpeed().y);
        _batStateManager.transform.position = Vector2.MoveTowards(_batStateManager.transform.position, _batStateManager.GetSleepPos().position, maxDistance * Time.deltaTime);
        CeilInCheck();
        //Xử lý việc bay về chỗ ngủ và chuyển trạng thái Ceil In
    }

    private void HandleFlipSprite()
    {
        hasFlip = true;

        if (_batStateManager.transform.position.x > _batStateManager.GetSleepPos().position.x)
            _batStateManager.FlipLeft();
        else if (_batStateManager.transform.position.x < _batStateManager.GetSleepPos().position.x)
            _batStateManager.FlipRight();
        //Xử lý lật sprite trước khi bay về chỗ ngủ
    }

    private void CeilInCheck()
    {
        if (Vector2.Distance(_batStateManager.transform.position, _batStateManager.GetSleepPos().position) < 0.01f)
        {
            _batStateManager.ChangState(_batStateManager.batCeilInState);
            hasFlyPatrol = false;
        }
    }

    public override void FixedUpdate() { }
}
