﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    protected Animator _anim;
    protected string _ID;

    [Tooltip("Với vài GObj cần Tutor thì để ý 2 phần này, còn kh thì bỏ qua. Nó sẽ " +
        "Link GObj này với Tutor để tắt Tutor khi Player đã tác động lên GObj này")]
    [SerializeField] protected bool _needTutor;
    [SerializeField] protected GameObject _tutorRef;

    [Header("Special Obj?"), Tooltip("Tick vào và chọn skill nếu đây là obj đặc biệt, " +
    "cung cấp skill cho Player")]
    [SerializeField] protected bool _isApplySkillToPlayer;
    [SerializeField] protected GameEnums.EPlayerState _skillUnlocked;
    [SerializeField] protected float _skillUnlockDelay;
    //Nếu là obj đặc biệt sẽ Notify events unlock skill cho Player

    public Animator Animator { get { return _anim; } }

    public string ID { get { return _ID; } }

    //Awake should be use as constructor
    protected virtual void Awake()
    {
        GetReferenceComponents();
        HandleObjectState();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetUpProperties();
    }

    protected virtual void GetReferenceComponents()
    {
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Hàm dưới để xử lý gameobject này nếu có tồn tại các key đặc biệt trong
    /// PlayerPrefs: Deleted, Disabled, ...
    /// Để việc chơi lại (Load lại scene) sẽ giữ nguyên hiện trạng chứ 0 reload lại toàn bộ
    /// </summary>

    protected virtual void HandleObjectState()
    {
        _ID = gameObject.name;

        if (PlayerPrefs.HasKey(GameEnums.ESpecialStates.Deleted + _ID))
            Destroy(gameObject);
        //Debug.Log("ID: " + _ID);
    }

    protected virtual void SetUpProperties() { }

    protected IEnumerator NotifyUnlockSkill()
    {
        yield return new WaitForSeconds(_skillUnlockDelay);

        EventsManager.Instance.NotifyObservers(GameEnums.EEvents.PlayerOnUnlockSkills, _skillUnlocked);
        PlayerPrefs.SetString(GameEnums.ESpecialStates.SkillUnlocked + _skillUnlocked.ToString(), "Unlocked");
    }

}
