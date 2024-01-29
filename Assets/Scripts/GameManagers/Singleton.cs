using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //Docs: In a generic type or method definition, a type parameter
    //is a PLACEHOLDER for a specific type that a client specifies
    //when they create an instance of the generic type.

    protected static T _instance = null;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                if (FindObjectOfType<T>() != null)
                    _instance = FindObjectOfType<T>();
                else
                    new GameObject().AddComponent<T>().name = "Singleton_" + typeof(T).ToString();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        //Xem lai ?
        if (_instance && _instance.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            Destroy(gameObject);
        else
            _instance = GetComponent<T>();
    }
}