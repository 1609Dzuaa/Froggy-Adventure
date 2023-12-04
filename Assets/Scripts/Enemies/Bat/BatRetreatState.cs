using UnityEngine;

public class BatRetreatState : MEnemiesBaseState
{
    private BatManager _batManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _batManager = (BatManager)charactersManager;
        _batManager.Animator.SetInteger("state", (int)EnumState.BatState.retreat);
        Debug.Log("Rt");
    }

    public override void ExitState() { }

    public override void Update() 
    {
        //Cập nhật vị trí bay về
        _batManager.transform.position = Vector2.MoveTowards(_batManager.transform.position, _batManager.SleepPos.position, _batManager.GetChaseSpeed() * Time.deltaTime);

        if (CheckIfBackHome(_batManager.transform.position, _batManager.SleepPos.position))
            _batManager.ChangeState(_batManager.BatCeilInState);
    }

    private bool CheckIfBackHome(Vector2 batPos, Vector2 sleepPos)
    {
        if (Vector2.Distance(batPos, sleepPos) < 0.1f)
            return true;
        return false;
        //Check nếu vị trí vừa đc cập nhật có RẤT GẦN với vị trí ngủ kh
    }

    public override void FixedUpdate() { }
}
