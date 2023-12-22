using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeController : MonoBehaviour
{
    [SerializeField] private Transform collectedEffect;

    //Giảm tải bớt khối lượng dòng code cho class PlayerManager
    //Run faster when eat

    private void OnEnable()
    {
        PlayerStateManager.OnAppliedBuff += ApplyBuff;
    }

    private void ApplyBuff()
    {
        //how to apply buff for player from this shit ?
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            var playerManagerScript = collision.gameObject.GetComponent<PlayerStateManager>();
            playerManagerScript.IncreaseOrangeCount();
            playerManagerScript.GetScoreText().text = "Oranges:" + playerManagerScript.GetOrangeCount().ToString();
            playerManagerScript.GetCollectSound().Play();
            Instantiate(collectedEffect, transform.position, Quaternion.identity, null);
            Destroy(this.gameObject);
        }
    }*/
}
