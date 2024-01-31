using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeckoManager : MEnemiesManager
{
    //Xử lý xong vấn đề sprite size của Gecko:
    //Đối với các sprite có size khác bthg trong 1 sprite sheet thì điều chỉnh pivot
    //trong Sprite Editor của sprite đó
    //https://www.reddit.com/r/Unity2D/comments/2qtnzm/animating_sprites_of_different_sizes/

    [Header("Teleport Distance")]
    [SerializeField] private float _teleDistance;

    [Header("Range")]
    [SerializeField] private Vector2 _damageRange;
    [SerializeField] private Vector2 _teleportableRange;

    private Transform _playerRef;
    private GeckoIdleState _geckoIdleState = new();
    private GeckoPatrolState _geckoPatrolState = new();
    private GeckoHideState _geckoHideState = new();
    private GeckoAttackState _geckoAttackState = new();
    private bool _canTeleport2Sides;

    //Chia nó thành 2 phần: TRÁI/PHẢI
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

    public GeckoIdleState GetGeckoIdleState() { return _geckoIdleState; }

    public GeckoPatrolState GetGeckoPatrolState() { return _geckoPatrolState; }

    public GeckoHideState GetGeckoHideState() { return _geckoHideState; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _playerRef = FindObjectOfType<PlayerStateManager>().transform;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetUpProperties()
    {
        _mEnemiesIdleState = _geckoIdleState;
        base.SetUpProperties();
    }

    protected override void Update()
    {
        UpdateCheckTeleportablePosition();
        CheckIfCanTeleport2Sides();
        base.Update();
        //Debug.Log("has DtP: " + _hasDetectedPlayer);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void HandleTeleportAndFlipSprite()
    {
        //Xử lý việc tele:
        //Có thể tele trước/sau player nếu TeleableRange(vùng quanh Player) 0 detect ra Ground/Wall
        //Nếu TeleableRange detect ra G/W thì xử lý sau:
        //Giữ nguyên 0 tele hoặc tele ra đằng sau NẾU bị player ép vào góc chết
        int isTeleport = Random.Range(0, 2); //0: 0 tele | 1: Tele ra đằng sau
        Vector3 newPos;

        if (isTeleport > 0 && _canTeleport2Sides)
        {
            FlippingSprite();
            if (_isFacingRight)
                newPos = new Vector3(_playerRef.position.x - _teleDistance, transform.position.y, 0f);
            else
                newPos = new Vector3(_playerRef.position.x + _teleDistance, transform.position.y, 0f);
        }
        else if(!_canTeleport2Sides)
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
        if (Physics2D.OverlapBox(transform.position, _damageRange, 0f, _enemiesSO.PlayerLayer))
        {
            //Vì thằng Overlap nó check cả 2 hướng trái/phải Gecko nên thêm đk dưới
            //để tránh việc dù Player ở sau lưng Gecko lúc nó attack
            //nhưng vẫn dính đòn
            if (_hasDetectedPlayer)
            {
                SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.GeckoAttackSfx, 1.0f);
                EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnTakeDamage, _isFacingRight);
            }
        }

        //Cũng là Event của animation Attack lúc vụt lưỡi
    }

    private void UpdateCheckTeleportablePosition()
    {
        _teleAbleLeftPos = new Vector2(_playerRef.position.x - _teleportableRange.x / 2, _playerRef.position.y);
        _teleAbleRightPos = new Vector2(_playerRef.position.x + _teleportableRange.x / 2, _playerRef.position.y);
    }

    private bool CheckIfCanTeleport2Sides()
    {
        _collideLeft = Physics2D.OverlapBox(_teleAbleLeftPos, _teleportableRange, 0f, _mEnemiesSO.WallLayer);
        _collideRight = Physics2D.OverlapBox(_teleAbleRightPos, _teleportableRange, 0f, _mEnemiesSO.WallLayer);

        return _canTeleport2Sides = !_collideLeft && !_collideRight;
    }

    private void ChangeToAttack()
    {
        if (BuffsManager.Instance.GetTypeOfBuff(GameEnums.EBuffs.Invisible).IsAllowToUpdate)
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
    }

    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(_enemiesSO.AttackDelay);

        ChangeState(_geckoHideState);
    }

    private void SpawnDisappearEffect()
    {
        GameObject disEff = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.GeckoDisappear);
        disEff.SetActive(true);
        disEff.GetComponent<EffectController>().SetPosition(transform.position);
    }

    private void SpawnAppearEffect()
    {
        GameObject disEff = Pool.Instance.GetObjectInPool(GameEnums.EPoolable.GeckoAppear);
        disEff.SetActive(true);
        disEff.GetComponent<EffectController>().SetPosition(transform.position);
    }
}
