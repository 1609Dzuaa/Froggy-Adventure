using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//refactor ?
public abstract class PlayerBuffs : MonoBehaviour
{
    protected bool _isActivating;
    protected float _entryTime;

    public bool IsActivating { get => _isActivating; set => _isActivating = value; }

    public virtual void Awake() { }

    public virtual void Start() { }

    //public virtual void Update() { }
}
