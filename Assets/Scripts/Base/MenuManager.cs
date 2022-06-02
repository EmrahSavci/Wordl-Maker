using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MenuManager : Singleton<MenuManager>
{
    [HideInInspector]
    public StartMenu StartMenu;
    [HideInInspector]

    public PlayTimeMenu PlayTimeMenu;
    [HideInInspector]
    public PauseMenu PauseMenu;
    [HideInInspector]
    public FinishMenu FinishMenu;
    [HideInInspector]
    public MarketPlaceMenu MarketPlaceMenu;
    private Canvas canvas;

    public void Setup()
    {
        StartMenu = FindObjectOfType<StartMenu>();
        PlayTimeMenu = FindObjectOfType<PlayTimeMenu>();
        PauseMenu = FindObjectOfType<PauseMenu>();
        FinishMenu = FindObjectOfType<FinishMenu>();
        MarketPlaceMenu = FindObjectOfType<MarketPlaceMenu>();
        canvas = PlayTimeMenu.transform.GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        EventManager.FirstTouch += ShowPlayTimeMenu;
        EventManager.OnPause += Pause;
        EventManager.FinishGame += WhenFinish;
        EventManager.OnAfterLoadedLevel += ShowStartMenu;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.FirstTouch -= ShowPlayTimeMenu;
        EventManager.FinishGame -= WhenFinish;
        EventManager.OnPause -= Pause;
        EventManager.OnAfterLoadedLevel -= ShowStartMenu;
    }
    public void ShowStartMenu()
    {
        StartMenu.Show();
    }

    public void ShowPlayTimeMenu()
    {
        PlayTimeMenu.Show();
    }

    private void WhenFinish(GameStat stat)
    {
        StartMenu.Hide();
        PlayTimeMenu.Hide();
        PauseMenu.Hide();
    }

    private void Pause(bool pause)
    {
        if (pause)
        {
            GameBase.Instance.ChangeStat(GameStat.Paused);
            Time.timeScale = 0;
            PauseMenu.Show();
        }
        else
        {
            GameBase.Instance.ChangeStat(GameStat.Playing);
            Time.timeScale = 1;
            PauseMenu.Hide();
        }
    }



}

public static class ExtensionMenuManager
{
    public static void Show(this Menus menu)
    {
        switch (menu)
        {
            case Menus.MarketMenu:
                MenuManager.Instance.MarketPlaceMenu.Show();
                break;
            case Menus.PauseMenu:
                break;
            case Menus.GameOverMenu:
                break;
        }
     
    }
    
    public static void Hide(this Menus menu)
    {
        switch (menu)
        {
            case Menus.MarketMenu:
                MenuManager.Instance.MarketPlaceMenu.Hide();
                break;
            case Menus.PauseMenu:
                break;
            case Menus.GameOverMenu:
                break;
        }
     
    }

}
