using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManager : MEnemiesManager
{
    [Header("Safe Check")]
    [SerializeField] private Transform _safeCheck;
    [SerializeField] private float _safeCheckDistance;
    private bool _isDetected;

    private MrAttackState _mrAttackState = new();

    public bool IsDetected { get { return _isDetected; } }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        MEnemiesAttackState = _mrAttackState; //Convert EnemiesAttack sang MushroomAttack
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SafeCheck();
        DrawRaySafeCheck();
        //Debug.Log("HW: " + _isDetected);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void SafeCheck()
    {
        if (PlayerShieldBuff.Instance.IsAllowToUpdate)
            return;

        if (!_isFacingRight)
            _isDetected = Physics2D.Raycast(new Vector2(_safeCheck.position.x, _safeCheck.position.y), Vector2.right, _safeCheckDistance, _playerLayer);
        else
            _isDetected = Physics2D.Raycast(new Vector2(_safeCheck.position.x, _safeCheck.position.y), Vector2.left, _safeCheckDistance, _playerLayer);
    }

    private void DrawRaySafeCheck()
    {
        if (_isDetected)
        {
            if (!_isFacingRight)
                Debug.DrawRay(_safeCheck.position, Vector2.right * _safeCheckDistance, Color.yellow);
            else
                Debug.DrawRay(_safeCheck.position, Vector2.left * _safeCheckDistance, Color.yellow);
        }
        else
        {
            if (!_isFacingRight)
                Debug.DrawRay(_safeCheck.position, Vector2.right * _safeCheckDistance, Color.blue);
            else
                Debug.DrawRay(_safeCheck.position, Vector2.left * _safeCheckDistance, Color.blue);
        }
    }

    //Hàm này dùng để Invoke trong state Attack
    private void AllowUpdateAttack()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible))
        {
            ChangeState(MEnemiesIdleState);
            return;
        }

        _mrAttackState.SetAllowUpdate(true);
    }

}
