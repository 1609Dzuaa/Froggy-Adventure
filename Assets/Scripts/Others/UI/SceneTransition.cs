using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private void ChangeTransitionCanvasOrder()
    {
        UIManager.Instance.DecreaseTransitionCanvasOrder();
        UIManager.Instance.PopUpHPCanvas();
        //Của animation Fade Out - tránh các button của canvas bị đè bởi Black Scr
    }

    private void PopUpHPCanvas()
    {
        UIManager.Instance.PopUpHPCanvas();
        //Của animation Fade Out đoạn giữa - popup HP lên khi load PlayScene
    }
}
