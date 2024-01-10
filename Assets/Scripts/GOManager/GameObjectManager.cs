using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    protected Animator _anim;

    public Animator Animator { get { return _anim; } }

    //Awake should be use as constructor
    protected virtual void Awake()
    {
        GetReferenceComponents();
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

    protected virtual void SetUpProperties()
    {

    }

}
