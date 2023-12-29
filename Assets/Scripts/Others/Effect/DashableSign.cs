using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashableSign : MonoBehaviour
{
    [SerializeField] Transform _refPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = _refPosition.position;    
    }

    public void SetRefPosition(Transform refPos)
    {
        _refPosition = refPos;
    }
}
