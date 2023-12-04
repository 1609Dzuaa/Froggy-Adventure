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
        _batManager.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;
        lastRotateTime = Time.time;
    }

    public override void ExitState() { }

    public override void Update() 
    {
        if (Time.time - lastRotateTime >= _batManager.TimeEachRotate)
        {
            Zdegree -= _batManager.DegreeEachRotation;
            _batManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }

    public override void FixedUpdate() { }
}
