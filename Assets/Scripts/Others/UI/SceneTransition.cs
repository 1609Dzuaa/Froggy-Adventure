using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private void ChangeTransitionCanvasOrder()
    {
        UIManager.Instance.DecreaseTransitionCanvasOrder();
        //Của animation Fade Out - tránh các button của canvas bị đè bởi Black Scr
    }
}
