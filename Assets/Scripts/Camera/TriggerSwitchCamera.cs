using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitchCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _prevCamera;
    [SerializeField] CinemachineVirtualCamera _camNeedToSwitch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            CameraController.Instance.SwitchingCamera(_camNeedToSwitch);
            //Debug.Log("Switched new cam");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CameraController.Instance.SwitchingCamera(_prevCamera);
        //Debug.Log("Switch to prev cam");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConstants.PLAYER_TAG))
        {
            CameraController.Instance.SwitchingCamera(_camNeedToSwitch);
            //Debug.Log("Switched new cam");
        }
    }
}
