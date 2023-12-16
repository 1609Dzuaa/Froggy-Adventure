using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            var playerScript = collision.GetComponent<PlayerStateManager>();
            playerScript.ChangeState(playerScript.gotHitState);
            playerScript.GetRigidBody2D().AddForce(new Vector2(playerScript.GetKnockBackForce(), 0f));
        }
    }
}
