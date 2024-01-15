using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/BulletStats")]
public class BulletStats : ScriptableObject
{
    [Header("Speed")]
    [SerializeField] private float _bulletSpeed;

    [Header("Time")]
    [SerializeField] private float _existTime;

    #region GETTER

    public float BulletSpeed { get => _bulletSpeed; }

    public float ExistTime { get => _existTime; }

    #endregion
}
