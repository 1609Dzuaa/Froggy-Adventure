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
    //Coi lại vẫn có thể tele mấy góc chết
    //Solution: nếu vị trí tele mới rơi vào góc chết thì giữ nguyên vị trí tele

    [Header("Teleport Distance")]
    [SerializeField] private float _teleDistance;
    [SerializeField] private float _teleportableDistance;

    [Header("Player Reference")]
    [SerializeField] private Transform _playerRef;

    [Header("Range")]
    [SerializeField] private Vector2 _damageRange;
    [SerializeField] private Vector2 _teleportableRange;
    [SerializeField] private Transform _wallCheck2;
    [SerializeField] private float _distToWall;

    private GeckoIdleState _geckoIdleState = new();
    private GeckoPatrolState _geckoPatrolState = new();
    private GeckoHideState _geckoHideState = new();
    private GeckoAttackState _geckoAttackState = new();
    private GeckoGotHitState _geckoGotHitState = new();
    private bool _canTele2Ways;

    //mò mãi đéo xác định đc collision side khi dùng overlapbox với playerRef
    //nên chia nó thành 2 phần: TRÁI/PHẢI
    //dời thằng center từ playerRef sang 2 pos mới là playerRefPos +- teleAbleRange / 2
    //TeleportableRange như mô tả ở dưới:
    //    |===_playerRefPos===|
    // => Chuyển nó thành như này:
    // |=leftPos=_playerRefPos=rightPos=|
    //và thực hiện việc check Overlap của 2 thằng left/right

    private Vector2 _teleAbleLeftPos;
    private Vector2 _teleAbleRightPos;
    private bool _collideRight;
    private bool _collideLeft;
    private Rect _teleAbleRect;

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
        //WallCheck2();
        //DrawWallCheck2();
        UpdateCheckTeleportablePosition();
        CheckIfCanTeleport2Ways();
        //Debug.Log("cantele2ways: " + _canTele2Ways);
        base.Update();
        //Collider2D col2D = Physics2D.OverlapBox(_playerRef.position, _teleportableRange, 0f, _wallLayer);



        /*Collider2D leftCollider = Physics2D.OverlapBox(_teleAbleLeftPos, _teleportableRange, 0f, _wallLayer);
        Collider2D rightCollider = Physics2D.OverlapBox(_teleAbleRightPos, _teleportableRange, 0f, _wallLayer);

        if (leftCollider != null)
        {
            Debug.Log("va trai");
        }
        if (rightCollider != null)
        {
            Debug.Log("va phai");
        }*/
        /*Collider2D col2D = Physics2D.OverlapBox(_playerRef.position, _teleportableRange, 0f, _wallLayer);
        if (col2D)
        {
            Rect rectBox = new Rect(_playerRef.position, _teleportableRange);

            Debug.Log("rect size: " + rectBox.size.x);
            if (rectBox.xMax >= col2D.bounds.center.x && rectBox.xMin < col2D.bounds.center.x)
                Debug.Log("va cham phai");
            else if (rectBox.xMin > col2D.bounds.center.x)
                Debug.Log("va cham trai");

            //prob still here
            /*if (rectBox.xMax > transform.position.x && transform.position.x > _playerRef.position.x)
                Debug.Log("Bi kep phai");
            else if (rectBox.xMin < transform.position.x && transform.position.x < _playerRef.position.x)
                Debug.Log("Bi kep trai");
         }*/
    }

    /*private void WallCheck2()
    {
        if (_isFacingRight)
            _wallHit = Physics2D.Raycast(_wallCheck2.position, Vector2.right * _distToWall, _wallLayer);
        else
            _wallHit = Physics2D.Raycast(_wallCheck2.position, Vector2.left * _distToWall, _wallLayer);
    }

    private void DrawWallCheck2()
    {
        if (_isFacingRight)
        {
            if (_wallHit)
                Debug.DrawRay(_wallCheck2.position, Vector2.right, Color.red);
            else
                Debug.DrawRay(_wallCheck2.position, Vector2.right, Color.green);
        }
        else
        {
            if (_wallHit)
                Debug.DrawRay(_wallCheck2.position, Vector2.left, Color.red);
            else
                Debug.DrawRay(_wallCheck2.position, Vector2.left, Color.green);
        }
    }*/

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void HandleTeleportAndFlipSprite()
    {
        //Cách tele hiện tại của con vk này phụ thuộc quá vào Player
        //Nên tự tele vì có thế nhiều lúc Player vào góc khó/chết
        //Khiến con vk này tele vào mấy chỗ đó => bug toè mồm
        //Physics2D.OverlapBox(_playerRef.position, _teleportableRange, 0f, _wallLayer).bounds.max;
        //Collider2D col2D = Physics2D.OverlapBox(_playerRef.position, _teleportableRange, 0f, _wallLayer);
        //Rect rectBox = new Rect(_playerRef.position, _teleportableRange);
        int isTeleport = Random.Range(0, 2); //0: 0 tele | 1: Tele ra đằng sau
        //Debug.Log(isTeleport);
        Vector3 newPos;
        /*if (col2D)
        {
            /*if (col2D.bounds.center.x < _playerRef.transform.position.x)
                Debug.Log("va trai");
            else 
                Debug.Log("va phai");*/
        /*if (transform.position.x < col2D.bounds.max.x && transform.position.x > _playerRef.position.x)
            Debug.Log("dang bi kep ben phai");
        else if (transform.position.x > col2D.bounds.min.x && transform.position.x < _playerRef.position.x)
            Debug.Log("dang bi kep ben trai");
    }*/

        if (isTeleport > 0 && _canTele2Ways)
        {
            FlippingSprite();
            if (_isFacingRight)
                newPos = new Vector3(_playerRef.position.x - _teleDistance, transform.position.y, 0f);
            else
                newPos = new Vector3(_playerRef.position.x + _teleDistance, transform.position.y, 0f);
        }
        else if(!_canTele2Ways)
        {
            //Bị player dồn vào góc thì tele ra sau lưng player
            if (_collideLeft) //chạm trái
            {
                if (transform.position.x < _playerRef.position.x)
                {
                    FlippingSprite();
                    Debug.Log("kep trai, can tele sau");
                }
            }
            else if (_collideRight)
            {
                if (transform.position.x > _playerRef.position.x)
                {
                    FlippingSprite();
                    Debug.Log("kep PHAI, can tele sau");
                }
            }

            if (_isFacingRight)
                newPos = new Vector3(_playerRef.position.x - _teleDistance, transform.position.y, 0f);
            else
                newPos = new Vector3(_playerRef.position.x + _teleDistance, transform.position.y, 0f);
        }
        else //isTele = 0 => deo tele
        {
            //Coi lại teleDist
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
            if (_hasDetectedPlayer)
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

    private void UpdateCheckTeleportablePosition()
    {
        _teleAbleRect = new Rect(_playerRef.position, _teleportableRange * 2);
        _teleAbleLeftPos = new Vector2(_playerRef.position.x - _teleportableRange.x / 2, _playerRef.position.y);
        _teleAbleRightPos = new Vector2(_playerRef.position.x + _teleportableRange.x / 2, _playerRef.position.y);
    }

    private bool CheckIfCanTeleport2Ways()
    {
        //Như này chắc ổn r, đéo cần cầu kì, phức tạp thêm
        _collideLeft = Physics2D.OverlapBox(_teleAbleLeftPos, _teleportableRange, 0f, _wallLayer);
        _collideRight = Physics2D.OverlapBox(_teleAbleRightPos, _teleportableRange, 0f, _wallLayer);

        return _canTele2Ways = !_collideLeft && !_collideRight;
        //return _canTele2Ways = !Physics2D.OverlapBox(_teleAbleLeftPos, _teleportableRange, 0f, _wallLayer) && !Physics2D.OverlapBox(_teleAbleLeftPos, _teleportableRange, 0f, _wallLayer);
        //return _canTele2Ways = !Physics2D.OverlapBox(_playerRef.position, _teleportableRange, 0f, _wallLayer);
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
        Gizmos.DrawWireCube(_teleAbleRightPos, _teleportableRange);
        Gizmos.DrawWireCube(_teleAbleLeftPos, _teleportableRange);
        //Gizmos.DrawWireCube(_playerRef.position, _teleportableRange);
        //Collider2D col2D = Physics2D.OverlapBox(_playerRef.position, _teleportableRange, 0f, _wallLayer);
        //Gizmos.DrawWireCube(_playerRef.position, _teleportableRange);
    }

    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(_attackDelay);

        ChangeState(_geckoHideState);
    }

    private void SpawnDisappearEffect()
    {
        GameObject disEff = EffectPool.Instance.GetObjectInPool(GameConstants.GECKO_DISAPPEAR_EFFECT);
        disEff.SetActive(true);
        disEff.GetComponent<EffectController>().SetPosition(transform.position);
    }

    private void SpawnAppearEffect()
    {
        GameObject disEff = EffectPool.Instance.GetObjectInPool(GameConstants.GECKO_APPEAR_EFFECT);
        disEff.SetActive(true);
        disEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
