using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void PopUpSettingsPanel()
    {

    }

    public void PopUpCreditsPanel()
    {

    }
}
