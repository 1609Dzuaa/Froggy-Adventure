using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    protected Animator _anim;

    public Animator Animator { get { return _anim; } }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _anim = GetComponent<Animator>();
    }

}
