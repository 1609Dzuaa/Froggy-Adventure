using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/EnemiesStats")]
public class EnemiesStats : ScriptableObject
{
    [Header("Player Check")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _playerCheckDistance;

    [Header("Wall Check")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _wallCheckDistance;

    [Header("Force")]
    [SerializeField] private Vector2 _knockForce;

    [Header("Time")]
    [SerializeField] private float _restTime;
    [SerializeField] private float _patrolTime;
    [SerializeField] private float _attackDelay;

    [Header("Speed")]
    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _chaseSpeed;

    [Header("Z Rotation When Dead")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _timeEachRotate;

    public LayerMask PlayerLayer { get { return _playerLayer; } }

    public float PlayerCheckDistance { get { return _playerCheckDistance; } }

    public LayerMask WallLayer { get { return _wallLayer; } }

    public float WallCheckDistance { get { return _wallCheckDistance; } }

    public Vector2 KnockForce { get { return _knockForce; } }

    public float RestTime { get { return _restTime; } }

    public float PatrolTime { get { return _patrolTime; } }

    public float AttackDelay { get { return _attackDelay; } }

    public float PatrolSpeed { get { return _patrolSpeed; } }

    public float ChaseSpeed { get { return _chaseSpeed; } }
}
