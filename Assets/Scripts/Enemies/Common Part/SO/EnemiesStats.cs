using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/EnemiesStats")]
public class EnemiesStats : ScriptableObject
{
    [Header("Player Check")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _playerCheckDistance;

    [Header("Force")]
    [SerializeField] private Vector2 _knockForce;

    [Header("Time")]
    [SerializeField] private float _attackDelay;

    [Header("Z Rotation When Dead")]
    [SerializeField] private float _degreeEachRotation;
    [SerializeField] private float _timeEachRotate;

    public LayerMask PlayerLayer { get { return _playerLayer; } }

    public float PlayerCheckDistance { get { return _playerCheckDistance; } }

    public Vector2 KnockForce { get { return _knockForce; } }

    public float AttackDelay { get { return _attackDelay; } }

    public float DegreeEachRotation { get { return _degreeEachRotation ; } }

    public float TimeEachRotate { get { return _timeEachRotate; } }
}
