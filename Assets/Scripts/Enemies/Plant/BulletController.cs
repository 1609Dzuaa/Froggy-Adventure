using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float _bulletSpeed;

    [Header("Time")]
    [SerializeField] private float _existTime;

    [Header("Pieces & Position")]
    [SerializeField] private GameObject _piece1;
    [SerializeField] private GameObject _piece2;
    [SerializeField] private Transform _piece1Position;
    [SerializeField] private Transform _piece2Position;

    [Header("Horizontal Or Vertical")]
    [SerializeField] private bool _isHorizontal;

    private Rigidbody2D _rb;
    private float _entryTime;
    private bool _isDirectionRight = false;

    public void SetIsDirectionRight(bool para) { this._isDirectionRight = para; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _entryTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _entryTime >= _existTime)
        {
            //Spawn lá hoặc effect gì đấy
            //Debug.Log("Time Out");
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_isHorizontal)
        {
            if (_isDirectionRight)
                _rb.velocity = new Vector2(_bulletSpeed, 0f);
            else
                _rb.velocity = new Vector2(-_bulletSpeed, 0f);
        }
        else
            _rb.velocity = new Vector2(0, -_bulletSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Có nên thử cho nó damage allies của mình || box ?
        if(collision.collider.name == "Player" || collision.collider.CompareTag("Ground"))
        {
            if(collision.collider.name == "Player")
            {
                var playerScript = collision.collider.GetComponent<PlayerStateManager>();
                if (_isDirectionRight)
                    playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.KnockBackForce);
                else
                    playerScript.GetRigidBody2D().AddForce(playerScript.GetPlayerStats.KnockBackForce * new Vector2(-1f, 1f));

                playerScript.ChangeState(playerScript.gotHitState);
            }
            SpawnBulletPieces();
            Destroy(this.gameObject);
        }
        else if (collision.collider.CompareTag(GameConstants.SHIELD_TAG))
        {
            SpawnBulletPieces();
            Destroy(this.gameObject);
        }
    }

    public void SpawnBulletPieces()
    {
        GameObject[] pieces = new GameObject[2];
        pieces[0] = Instantiate(_piece1, _piece1Position.position, Quaternion.identity, null);
        pieces[1] = Instantiate(_piece2, _piece2Position.position, Quaternion.identity, null);
        for (int i = 0; i < pieces.Length; i++)
            pieces[i].GetComponent<BulletPieceController>().SetIsShotFromRight(_isDirectionRight);
    }
}
