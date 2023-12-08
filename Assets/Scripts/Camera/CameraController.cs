using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    //[SerializeField] float _timeEachShake;

    //private float _entryTime;

    /*private void Start()
    {
        _entryTime = Time.time; 
    }*/

    private void Update()
    {
        this.transform.position = new Vector3(player.position.x, player.position.y, player.position.z - 10);
        /*if(Time.time - _entryTime >= _timeEachShake)
        {
            Shake();
            _entryTime = Time.time;
        }*/   
    }

    /*private void Shake()
    {
        this.transform.position = new Vector3(player.position.x - 1.5f, player.position.y - 1.5f, player.position.z - 10);
    }*/
}
