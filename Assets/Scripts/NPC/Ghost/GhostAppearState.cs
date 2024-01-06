using UnityEngine;

public class GhostAppearState : CharacterBaseState
{
    private GhostManager _ghostManager;

    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _ghostManager = (GhostManager)charactersManager;
        _ghostManager.Animator.SetInteger("state", (int)GameEnums.EGhostState.appear);
        _ghostManager.transform.position = RandomAppearPosition();
        //Debug.Log("App");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private Vector2 RandomAppearPosition()
    {
        //Bỏ đi 1 lượng 0.5f tránh có thể random dính ngay min/max
        //=>Khi enter state wander có thể gặp lỗi 0 như ý
        return new Vector2(Random.Range(_ghostManager.LeftBound.position.x + 0.5f, _ghostManager.RightBound.position.x - 0.5f), _ghostManager.LeftBound.position.y);
    }
}
