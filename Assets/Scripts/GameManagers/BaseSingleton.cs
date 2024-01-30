using UnityEngine;

public abstract class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
                {
                    GameObject gObj = new GameObject();
                    gObj.AddComponent<T>();
                    gObj.name = "Singleton_" + typeof(T).ToString();
                    Debug.Log("Singleton created by getter: " + gObj.name);
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        CreateInstance();
    }

    protected void CreateInstance()
    {
        if (!_instance)
        {
            _instance = this as T;
            Debug.Log("DontDestroy: " + this);
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.Log("Destroy: " + this);
            Destroy(gameObject);
        }
    }
}