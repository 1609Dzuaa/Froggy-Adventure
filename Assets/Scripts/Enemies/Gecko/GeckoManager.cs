using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoManager : MEnemiesManager
{
    //Xử lý xong vấn đề sprite size của Gecko:
    //Đối với các sprite có size khác bthg trong 1 sprite sheet thì điều chỉnh pivot
    //trong Sprite Editor của sprite đó
    //https://www.reddit.com/r/Unity2D/comments/2qtnzm/animating_sprites_of_different_sizes/
    //Coi lại vẫn đụng wall

    [Header("Teleport Distance")]
    [SerializeField] private float _teleDistance;

    [Header("Player Reference")]
    [SerializeField] private Transform _playerRef;

    [Header("Damage Range")]
    [SerializeField] private Vector2 _damageRange;

    private GeckoIdleState _geckoIdleState = new();
    private GeckoPatrolState _geckoPatrolState = new();
    private GeckoHideState _geckoHideState = new();
    private GeckoAttackState _geckoAttackState = new();
    private GeckoGotHitState _geckoGotHitState = new();

    public GeckoIdleState GetGeckoIdleState() { return _geckoIdleState; }

    public GeckoPatrolState GetGeckoPatrolState() { return _geckoPatrolState; }

    public GeckoHideState GetGeckoHideState() { return _geckoHideState; }

    public GeckoAttackState GetGeckoAttackState() { return _geckoAttackState; }

    public GeckoGotHitState GetGeckoGotHitState() { return _geckoGotHitState; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        _state = _geckoIdleState;
        _state.EnterState(this);
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
        {
            if (_isFacingRight)
                newPos = new Vector3(_playerRef.position.x - _teleDistance, transform.position.y, 0f);
            else
                newPos = new Vector3(_playerRef.position.x + _teleDistance, transform.position.y, 0f);
        }

        transform.position = newPos;
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
        if (Physics2D.OverlapBox(transform.position, _damageRange, 0f, _playerLayer))
        {
            //Vì thằng Overlap nó check cả 2 hướng trái/phải Gecko nên thêm đk dưới
            //để tránh việc dù Player ở sau lưng Gecko lúc nó attack
            //nhưng vẫn dính đòn
            if(_hasDetectedPlayer)
            {
                var playerScript = _playerRef.GetComponent<PlayerStateManager>();
                if (_isFacingRight)
                    playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.KnockBackForce);
                else
                    playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.KnockBackForce * new Vector2(-1f, 1f));

                playerScript.ChangeState(playerScript.gotHitState);
                if (_isFacingRight)
                    playerScript.GetRigidBody2D().AddForce(new Vector2(playerScript.GetPlayerStats.KnockBackForce.x, 0f));
                else
                    playerScript.GetRigidBody2D().AddForce(new Vector2(-playerScript.GetPlayerStats.KnockBackForce.x, 0f));
                //Debug.Log("Damage");
            }
        }

        //Cũng là Event của animation Attack lúc vụt lưỡi
    }

    private void ChangeToAttack()
    {
        if (PlayerInvisibleBuff.Instance.IsAllowToUpdate)
        {
            ChangeState(_geckoIdleState);
            return;
        }

        ChangeState(_geckoAttackState);
        //Event của animation Hide
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _damageRange);
    }

    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(_attackDelay);

        ChangeState(_geckoHideState);
    }
}
