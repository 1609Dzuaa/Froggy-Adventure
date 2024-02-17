﻿using System.Security.Cryptography;
using UnityEngine;

public class MEnemiesGotHitState : MEnemiesBaseState
{
    protected float _lastRotateTime;
    protected float _Zdegree;
    public override void EnterState(CharactersManager charactersManager)
    {
        base.EnterState(charactersManager);
        _mEnemiesManager.Animator.SetInteger(GameConstants.ANIM_PARA_STATE, (int)GameEnums.EMEnemiesState.gotHit);
        HandleBeforeDestroy();
        //Debug.Log("GH");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (Time.time - _lastRotateTime >= _mEnemiesManager.EnemiesSO.TimeEachRotate)
        {
            _Zdegree -= _mEnemiesManager.EnemiesSO.DegreeEachRotation;
            _mEnemiesManager.transform.Rotate(0f, 0f, _Zdegree);
            _lastRotateTime = Time.time;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected virtual void HandleBeforeDestroy()
    {
        _mEnemiesManager.GetSpriteRenderer.sortingLayerName = GameConstants.RENDER_MAP_LAYER;
        _mEnemiesManager.GetSpriteRenderer.sortingOrder = GameConstants.RENDER_MAP_ORDER;
        _mEnemiesManager.GetRigidbody2D().velocity = Vector2.zero; //Cố định vị trí
        _mEnemiesManager.GetRigidbody2D().AddForce(_mEnemiesManager.EnemiesSO.KnockForce, ForceMode2D.Impulse);
        _mEnemiesManager.GetCollider2D.enabled = false;
        if (!_mEnemiesManager.NotPlayDeadSfx)
            SoundsManager.Instance.PlaySfx(GameEnums.ESoundName.EnemiesDeadSfx, 1.0f);
        if (!_mEnemiesManager.ID.Contains(GameConstants.CLONE))
            PlayerPrefs.SetString(GameEnums.ESpecialStates.Deleted + _mEnemiesManager.ID, "deleted");
    }
}
