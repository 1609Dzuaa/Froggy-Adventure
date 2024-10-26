using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingPlatformController : GameObjectManager
{
    [Header("Time")]
    [SerializeField] private float _disableDelay;
    [SerializeField] private float _enableDelay;
    [SerializeField] private float _tweenDuration;

    private bool _hasBeenStepOn;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;
    private Vector2 _initPosition;
    private int _initLayer;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void GetReferenceComponents()
    {
        base.GetReferenceComponents();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void SetUpProperties()
    {
        _initPosition = transform.position;
        _initLayer = gameObject.layer;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    /*void Update()
    {
        if(_hasBeenStepOn && _allowDisable)
        {
            _boxCollider2D.enabled = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG) && !_hasBeenStepOn)
        {
            _hasBeenStepOn = true;
            StartCoroutine(Disable());
        }
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(_disableDelay);

        _boxCollider2D.enabled = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _anim.SetBool("Off", true);
        //Set layer về mặc định nếu 0 nó vẫn giữ layer Ground
        //=> làm cho Player vẫn ở trên Ground dù platform đã bị disable
        gameObject.layer = 0;
        StartCoroutine(Enable());
    }

    private IEnumerator Enable()
    {
        yield return new WaitForSeconds(_enableDelay);

        _anim.SetBool("Off", false);
        _rb.DOMove(_initPosition, _tweenDuration).OnComplete(() =>
        {
            _boxCollider2D.enabled = true;
            gameObject.layer = _initLayer;
            _hasBeenStepOn = false;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        });
    }
}
