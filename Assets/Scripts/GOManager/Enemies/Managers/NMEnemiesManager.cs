using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMEnemiesManager : EnemiesManager
{
    //Rotate sprite after got hit
    [Header("Z Rotation When Dead")]
    [SerializeField] private float degreeEachRotation;
    [SerializeField] private float timeEachRotate;

    private NMEnemiesIdleState _nmEnemiesIdleState = new();
    private NMEnemiesAttackState _nmEnemiesAttackState = new();
    private NMEnemiesGotHitState _nmEnemiesGotHitState = new();

    public float GetTimeEachRotate() { return this.timeEachRotate; }

    public float GetDegreeEachRotation() { return this.degreeEachRotation; }

    public NMEnemiesIdleState NMEnemiesIdleState { get { return _nmEnemiesIdleState; } }

    public NMEnemiesAttackState NMEnemiesAttackState { get { return _nmEnemiesAttackState; } }

    public NMEnemiesGotHitState NMEnemiesGotHitState { get { return _nmEnemiesGotHitState; } }

    protected override void Start()
    {
        base.Start();
        _state = _nmEnemiesIdleState;
        _state.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log("update");
    }

    protected virtual void AllowAttackPlayer()
    {
        ChangeState(_nmEnemiesAttackState);
        //Debug.Log("Called");
        //Nhằm delay việc chuyển state Attack 
        //Tạo cảm giác enemies phản ứng rồi attack chứ 0 phải attack ngay lập tức
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
