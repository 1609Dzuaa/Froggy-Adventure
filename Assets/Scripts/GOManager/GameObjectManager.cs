using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class GameObjectManager : MonoBehaviour
{
    protected Animator _anim;
    protected string _ID;

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

        if (PlayerPrefs.HasKey(ESpecialStates.Deleted + _ID))
        {
            Debug.Log("Delete ID: " + _ID);
            Destroy(gameObject);
        }
        //Debug.Log("ID: " + _ID);
    }

    protected virtual void SetUpProperties() { }

    public void MarkAsDeleted()
    {
        string key = ESpecialStates.Deleted + ID;
        GameManager.Instance.ListPrefsInconsistentKeys.Add(key);
        PlayerPrefs.SetString(key, "deleted");
    }

}
