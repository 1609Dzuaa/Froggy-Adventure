using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHPController : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] private GameObject _parent;

    [Header("Boundaries")]
    [SerializeField] private Transform _topPos;
    [SerializeField] private Transform _botPos;

    [Header("Speed")]
    [SerializeField] private float _speedY;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= _topPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, _topPos.position.y, transform.position.z);
            _speedY = -_speedY;
        }
        else if (transform.position.y <= _botPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, _botPos.position.y, transform.position.z);
            _speedY = -_speedY;
        }
        transform.position += new Vector3(0f, _speedY, 0f) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            Destroy(_parent);
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.IncreaseHP();
        }
    }
}
