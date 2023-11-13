using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    //Jump higher when eat
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var playerManagerScript = collision.gameObject.GetComponent<PlayerStateManager>();
            playerManagerScript.IncreaseOrangeCount();
            playerManagerScript.GetScoreText().text = "Oranges:" + playerManagerScript.GetOrangeCount().ToString();
            playerManagerScript.GetCollectSound().Play();
            Destroy(this.gameObject);
        }
    }
}
