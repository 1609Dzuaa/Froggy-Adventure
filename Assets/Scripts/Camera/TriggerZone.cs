using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    //Object vô hình này sẽ là điểm mà khi Player trigger nó,
    //cam sẽ tự động điều chỉnh vị trí theo ý mình muốn

    [SerializeField] Transform _posNeedToMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !CameraController.GetInstance().HasTriggered)
        {
            CameraController.GetInstance().HasTriggered = true;
            CameraController.GetInstance().HasLeavedTriggerZone = false;
            CameraController.GetInstance().PosNeedToMove = _posNeedToMove;
            Debug.Log("Trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && CameraController.GetInstance().HasTriggered)
        {
            CameraController.GetInstance().HasTriggered = false;
            CameraController.GetInstance().HasLeavedTriggerZone = true;
            Debug.Log("Leave");
        }
    }
}
