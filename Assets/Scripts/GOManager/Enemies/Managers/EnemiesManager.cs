using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemiesManager : CharactersManager
{
    [Header("Player Check")]
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected float _checkDistance;
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected Vector2 _knockForce; //Knock Force khi bị hit
    protected bool _hasDetectedPlayer;
    protected bool _hasGotHit; //Đánh dấu bị Hit, tránh Trigger nhiều lần

    //Public Field
    public Vector2 KnockForce { get { return _knockForce; } }

    public bool HasDetectedPlayer { get { return _hasDetectedPlayer; } }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //Lấy anim và rb từ CharactersManager

    }

    protected void DestroyItSelf()
    {
        Destroy(this.gameObject);
    }

}
