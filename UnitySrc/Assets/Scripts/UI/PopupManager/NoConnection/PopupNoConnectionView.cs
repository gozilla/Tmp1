using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.UI;
using System;
using Project.UI.InGameMenu;
using Project.UI.MainMenu;

public class PopupNoConnectionView : PopupView
{
    [SerializeField]
    private InGameMenuController inGameMenuController = null;

    [SerializeField]
    private Loading loading = null;

    [SerializeField]
    private Button btnClose = null;

    [SerializeField]
    private Button btnReconnect = null;

    protected override void OnEnable()
    {
        base.OnEnable();

        btnClose.gameObject.SetActive(!loading.gameObject.activeInHierarchy);
        btnReconnect.gameObject.SetActive(loading.gameObject.activeInHierarchy);
        if (inGameMenuController.View.gameObject.activeInHierarchy)
        {
            inGameMenuController.View.OnEndGame();
            Project.App.AppData.Instance.ClearWebData();
        }

        if (loading.gameObject.activeInHierarchy)
            loading.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void Show()
    {
        gameObject.SetActive(true);        
    }

    public void Reconnect()
    {
        Project.App.AppData.Instance.ClearWebData();
        inGameMenuController.View.gameObject.SetActive(false);
        loading.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void Close()
    {
        gameObject.SetActive(false);        
    }

}
