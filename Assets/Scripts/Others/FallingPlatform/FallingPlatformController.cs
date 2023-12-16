using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : GameObjectManager
{
    [Header("Time")]
    [SerializeField] private float _disableDelay;

    private bool _hasBeenStepOn;
    private bool _allowDisable;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;

    protected override void Awake()
    {
        base.Awake();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(_hasBeenStepOn && _allowDisable)
        {
            _boxCollider2D.enabled = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !_hasBeenStepOn)
        {
            _hasBeenStepOn = true;
            StartCoroutine(Disable());
        }
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(_disableDelay);

        _allowDisable = true;
        _anim.SetTrigger("Off");
        //Set layer về mặc định nếu 0 nó vẫn giữ layer Ground
        //=> làm cho Player vẫn ở trên Ground dù platform đã bị disable
        gameObject.layer = 0;
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
