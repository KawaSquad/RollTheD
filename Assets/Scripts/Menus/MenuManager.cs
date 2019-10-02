﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("Instance not created");
            return instance;
        }
    }

    [SerializeField]
    private EMenuState currentMenuState;

    public List<MenuPanel> menus;
    MenuPanel currentMenu;

    [SerializeField]
    private SObject_Player mConfig_PlayerData;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(menus.Count == 0)
        {
            //get all if i am too lasy too fill the list manually
            GetAllPanel();
        }

        //replace all canvas
        ResetAllPanel();

        //apply first menu;
        ActiveState(currentMenuState);
    }

    public void GetAllPanel()
    {
        menus = new List<MenuPanel>();
        MenuPanel[] menuPanels = GameObject.FindObjectsOfType<MenuPanel>();
        for (int i = 0; i < menuPanels.Length; i++)
        {
            menus.Add(menuPanels[i]);
        }
    }

    public void ResetAllPanel()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].ResetPosition();
            menus[i].ActiveAlpha(false);
        }
    }

    public void ActiveState(EMenuState eMenuState)
    {
        currentMenuState = eMenuState;

        for (int i = 0; i < menus.Count; i++)
        {
            if(menus[i].menuState == eMenuState)
            {
                if (currentMenu != null)
                    currentMenu.ActiveAlpha(false);

                currentMenu = menus[i];
                currentMenu.ActiveAlpha(true);
                break;
            }
        }
    }

    public void ActiveOpenAccount(bool isMaster)
    {
        mConfig_PlayerData.Game_Master = isMaster;
        ActiveState(EMenuState.Account_Connection);
    }

    public void PreviousPanel()
    {
        switch (currentMenuState)
        {
            case EMenuState.Account_Connection:
                ActiveState(EMenuState.MainMenu);
                break;
            case EMenuState.Account_NewAccount:
                ActiveState(EMenuState.Account_Connection);
                break;
            case EMenuState.Account_Session:
                ActiveState(EMenuState.Account_Connection);
                //logout
                break;
            case EMenuState.Player_Character:
                ActiveState(EMenuState.Account_Connection);
                //Disconnected
                break;
            case EMenuState.GameMaster_Mode:
                ActiveState(EMenuState.Account_Connection);
                //logout
                break;
            case EMenuState.GameMaster_New_Session:
                ActiveState(EMenuState.Account_Session);
                //logout
                break;

            //case EMenuState.GameMaster_Session:
            //    break;
            //case EMenuState.GameMaster_Editor:
            //    break;
            default:
                Debug.LogError(currentMenuState.ToString() + " is not SET");
                break;
        }
    }

}

public enum EMenuState
{
    Unknow = 0,
    MainMenu = 1,
    Account_Connection = 2,
    Account_NewAccount = 3,
    Account_Session = 4,

    Player_Character = 13,

    GameMaster_Mode = 20,
    GameMaster_Editor = 21,
    GameMaster_New_Session = 22,

}
