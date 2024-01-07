using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBuffs : MonoBehaviour
{
    [SerializeField] protected float _buffDuration;
    protected float _entryTime;
    protected bool _isAllowToUpdate;

    public bool IsAllowToUpdate { get => _isAllowToUpdate; set => _isAllowToUpdate = value; }

    public virtual void Awake()
    { 
    }

    public virtual void Start()
    {
        //Xử lý các ref của buff đó (nếu có) trong đây
        //vd: disable th ref nào đó
    }

    public virtual void Update() { }

    public virtual void ApplyBuff()
    {
        //Xử lý cơ bản là bắt đầu bấm giờ và cho phép Update buff
        _entryTime = Time.time;
        _isAllowToUpdate = true;
        //Debug.Log("bat dau bam gio buff");
    }
}
