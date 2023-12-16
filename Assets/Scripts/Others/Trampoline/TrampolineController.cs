using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : GameObjectManager
{
    //Để ý mấy cái tag, layer name nữa

    [Header("Force Apply")]
    [SerializeField] private float _forceApply;

    [Header("Player Reference")]
    [SerializeField] private Transform _playerRef;

    [Header("Time")]
    [SerializeField] private float _restartTime;

    private bool _hasAddForce;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasAddForce)
        {
            _hasAddForce = true;
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.GetRigidBody2D().velocity = Vector2.up * _forceApply;
            //gặp chút vấn đề nếu AddForce thì 0 duy trì đc lực nhảy mạnh như
            //lúc ban đầu sau các lần nhảy kế tiếp

            //playerScript.GetRigidBody2D().AddForce(Vector2.up * _forceApply, ForceMode2D.Impulse);
            _anim.SetTrigger("Jump");
            StartCoroutine(RestartAddForceVar());
        }
    }

    private IEnumerator RestartAddForceVar()
    {
        yield return new WaitForSeconds(_restartTime);

        _hasAddForce = false;
    }

    private void SetIdleAnimation()
    {
        _anim.SetTrigger("Idle");
        //Event của Jump animation
    }

    private void ApplyForce()
    {
        var playerScript = _playerRef.GetComponent<PlayerStateManager>();
        playerScript.GetRigidBody2D().AddForce(new Vector2(0f, _forceApply), ForceMode2D.Impulse);
    }

}
