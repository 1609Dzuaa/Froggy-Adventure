using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool _instance;
    private List<GameObject> _pools = new List<GameObject>();
    private int _poolCount = 10;

    [SerializeField] private GameObject _bulletPrefabs;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _poolCount; i++)
        {
            GameObject obj = Instantiate(_bulletPrefabs);
            obj.SetActive(false);
            _pools.Add(obj);
        }
    }

    public GameObject GetPoolObject()
    {
        for (int i = 0; i < _pools.Count; i++)
        {
            if (!_pools[i].activeInHierarchy)
            {
                return _pools[i];
            }
        }

        return null;
    }
}
