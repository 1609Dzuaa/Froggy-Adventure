using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/MEnemiesStats")]
public class MEnemiesStats : ScriptableObject
{
    [Header("Wall Check")]
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance;

    [Header("Time")]
    [SerializeField] private float _restTime;
    [SerializeField] private float _patrolTime;

    [Header("Speed")]
    [SerializeField] private Vector2 _patrolSpeed;
    [SerializeField] private Vector2 _chaseSpeed;

    public float WallCheckDistance { get { return _wallCheckDistance; } }

    public float GroundCheckDistance { get { return _groundCheckDistance; } }

    public LayerMask WallLayer { get { return _wallLayer; } }

    public float RestTime { get { return _restTime; } }

    public float PatrolTime { get { return _patrolTime; } }

    public Vector2 PatrolSpeed { get { return _patrolSpeed; } }

    public Vector2 ChaseSpeed { get { return _chaseSpeed; } }

}
