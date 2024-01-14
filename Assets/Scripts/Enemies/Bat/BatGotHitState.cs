using UnityEngine;

public class BatGotHitState : MEnemiesGotHitState
{
    private float Zdegree = 0f;
    private float lastRotateTime;
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _batManager.GetRigidbody2D().gravityScale = 1f;
        lastRotateTime = Time.time;
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (Time.time - lastRotateTime >= _batManager.EnemiesSO.TimeEachRotate)
        {
            Zdegree -= _batManager.EnemiesSO.DegreeEachRotation;
            _batManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }

    public override void FixedUpdate() { }
}
