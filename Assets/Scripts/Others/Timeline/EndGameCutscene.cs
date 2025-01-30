using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCutscene : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _bossRoomCam;

    public void Fade()
    {
        _bossRoomCam.Priority = 3;
        UIManager.Instance.FadeEndGame();
    }
}
