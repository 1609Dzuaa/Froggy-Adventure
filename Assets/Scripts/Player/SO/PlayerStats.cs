using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Dùng SO để hạn chế việc Tight Coupling (Phụ thuộc chặt chẽ lẫn nhau giữa các class)
//và Generally speaking, the main purpose of SO is to store instances of data outside of a scene
//NOTE: "since all each script does is read from the player data asset."
//Docs:
//"Every time you instantiate that Prefab, it will get its own copy of that data.
//Instead of using this method, and storing duplicated data,
//you can use a ScriptableObject to store the data and then access it by
//reference from all of the Prefabs. This means that there is ONE copy of the data in memory."
//https://docs.unity3d.com/Manual/class-ScriptableObject.html
//https://gamedevbeginner.com/scriptable-objects-in-unity/#what_is_a_scriptable_object

//NOTE:
//Nên dùng SO dưới dạng (Read Only), hạn chế hoặc 0 modify trực tiếp nó trong quá trình Runtime
//https://www.reddit.com/r/Unity2D/comments/mayjhy/should_i_edit_scriptable_objects_at_runtime/
//https://forum.unity.com/threads/change-scriptableobject-at-runtime.1008376/

[CreateAssetMenu(fileName ="ScriptableObject", menuName = "ScriptableObject/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Speed")]
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    [SerializeField] private float _wallSlideSpeed;
    [SerializeField] private Vector2 _wallJumpSpeed;

    [Header("Force")]
    [SerializeField] private Vector2 _knockBackForce;
    [SerializeField] private Vector2 _jumpOnEnemiesForce;
    [SerializeField] private Vector2 _dashForce;

    //Đạt được nhiều thành tựu thì mới tăng thêm maxHP
    [Header("HP")]
    [SerializeField] private int _HP;
    [SerializeField] private int _maxHP;

    [Header("Factor")]
    [SerializeField, Range(1f, 2f)] private float _jumpSpeedFactor; 
    [SerializeField, Range(1f, 2f)] private float _dbJumpSpeedFactor; //Db jump 0 thể mạnh hơn Jump
    [SerializeField] private float _gravScale;

    [Header("Time")]
    //Disable Time Là khoảng thgian mà mình disable directionX ở state WallJump
    //Mục đích cho nó nhảy hướng ra phía đối diện cái wall vừa đu
    //Mà 0 bị ảnh hưởng bởi input directionX
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _disableTime;
    [SerializeField] private float _delayDashTime;
    [SerializeField] private float _invulnerableTime;
    [SerializeField] private float _timeEachApplyAlpha;

    [Header("Alpha Value")]
    [SerializeField] private float _alphaValueGotHit;

    public float SpeedX { get { return _speedX; } }

    public float SpeedY { get { return _speedY; } }

    public float WallSlideSpeed { get {  return _wallSlideSpeed; } }

    public Vector2 WallJumpSpeed { get { return _wallJumpSpeed; } }

    public Vector2 KnockBackForce { get { return _knockBackForce; } }

    public Vector2 JumpOnEnemiesForce { get { return _jumpOnEnemiesForce; } }

    public Vector2 DashForce { get { return _dashForce; } }

    public int MaxHP { get { return _maxHP; } }

    public float JumpSpeedFactor { get { return _jumpSpeedFactor; } }

    public float DbJumpSpeedFactor { get { return _dbJumpSpeedFactor; } }

    public float GravScale { get {  return _gravScale; } }

    public float JumpTime { get { return _jumpTime; } }

    public float DisableTime { get { return this._disableTime; } }

    public float DelayDashTime { get { return _delayDashTime; } }

    public float InvulnerableTime { get { return _invulnerableTime; } }

    public float TimeEachApplyAlpha { get { return _timeEachApplyAlpha; } }

    public float AlphaValueGotHit { get { return _alphaValueGotHit; } }

}
