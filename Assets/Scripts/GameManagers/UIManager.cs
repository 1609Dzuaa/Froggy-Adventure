using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingleton<UIManager>
{
    //Cần căn chỉnh pivot, anchors

    GameObject _creditsPanel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _creditsPanel = GameObject.FindWithTag("Credits");
        _creditsPanel.SetActive(false);
    }

    public void PopUpSettingsPanel()
    {

    }

    public void PopUpCreditsPanel()
    {
        _creditsPanel.SetActive(true);
    }
}
