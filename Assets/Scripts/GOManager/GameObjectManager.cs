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
        _anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

}
