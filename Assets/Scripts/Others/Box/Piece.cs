using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Force")]
    [SerializeField] private Vector2 force;
    [Header("Exist Time")]
    [SerializeField] private float time = 1.5f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(force);
        Invoke("DestroyItSelf", time);
    }

    private void DestroyItSelf()
    {
        Destroy(this.gameObject);
    }

}
