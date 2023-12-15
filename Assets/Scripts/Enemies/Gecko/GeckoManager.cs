using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoManager : MEnemiesManager
{
    //Xử lý xong vấn đề sprite size của Gecko:
    //Đối với các sprite có size khác bthg trong 1 sprite sheet thì điều chỉnh pivot
    //trong Sprite Editor của sprite đó
    //https://www.reddit.com/r/Unity2D/comments/2qtnzm/animating_sprites_of_different_sizes/
    //Có bug khi vụt Player thì bị tác vướng cái box collider chính mình nên bị văng ra @@

    [Header("Teleport Distance")]
    [SerializeField] private float _teleDistance;

    [Header("Player Reference")]
    [SerializeField] private Transform _playerRef;

    private GeckoIdleState _geckoIdleState = new();
    private GeckoPatrolState _geckoPatrolState = new();
    private GeckoHideState _geckoHideState = new();
    private GeckoAttackState _geckoAttackState = new();
    private GeckoGotHitState _geckoGotHitState = new();
    private BoxCollider2D _boxCollider;

    public GeckoIdleState GetGeckoIdleState() { return _geckoIdleState; }

    public GeckoPatrolState GetGeckoPatrolState() { return _geckoPatrolState; }

    public GeckoHideState GetGeckoHideState() { return _geckoHideState; }

    public GeckoAttackState GetGeckoAttackState() { return _geckoAttackState; }

    public GeckoGotHitState GetGeckoGotHitState() { return _geckoGotHitState; }

    public BoxCollider2D GetBoxCollider2D() { return _boxCollider; }

    protected override void Awake()
    {
        base.Awake();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        _state = _geckoIdleState;
        _state.EnterState(this);
        _boxCollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void HandleTeleportAndFlipSprite()
    {
        int isTeleport = Random.Range(0, 2); //0: 0 tele | 1: Tele ra đằng sau
        //Debug.Log(isTeleport);
        Vector3 newPos;
        if (isTeleport > 0)
        {
            FlippingSprite();
            if (_isFacingRight)
                newPos = new Vector3(_playerRef.position.x - _teleDistance, transform.position.y, 0f);
            else
                newPos = new Vector3(_playerRef.position.x + _teleDistance, transform.position.y, 0f);
        }
        else
            newPos = transform.position + Vector3.zero;

        //Teleport
        transform.position = Vector2.Lerp(transform.position, newPos, 1f);
    }

    private void CheckAfterAttacked()
    {
        if (_hasDetectedPlayer)
            ChangeState(_geckoHideState);
        else
            ChangeState(_geckoIdleState);

        //Event của animation Attack
    }

    private void EnableDamagePlayer()
    {
        _boxCollider.enabled = true;

        //Cũng là Event của animation Attack lúc vụt lưỡi
    }

    private void ChangeToAttack()
    {
        ChangeState(_geckoAttackState);

        //Event của animation Hide
    }

}
