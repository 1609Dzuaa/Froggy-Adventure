using UnityEngine;

public class BunnyGotHitState : MEnemiesGotHitState
{
    private float Zdegree = 0f;
    private float lastRotateTime;
    private BunnyManager _bunnyManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _bunnyManager = (BunnyManager)charactersManager;
        _bunnyManager.GetRigidbody2D().bodyType = RigidbodyType2D.Dynamic;
        lastRotateTime = Time.time;
    }

    public override void ExitState() { }

    public override void Update()
    {
        if (Time.time - lastRotateTime >= _bunnyManager.GetTimeEachRotate())
        {
            Zdegree -= _bunnyManager.GetDegreeEachRotation();
            _bunnyManager.transform.Rotate(0f, 0f, Zdegree);
            lastRotateTime = Time.time;
        }
    }

    public override void FixedUpdate() { }
}
