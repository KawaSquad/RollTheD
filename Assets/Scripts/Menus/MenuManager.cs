using System.Collections;
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
    private EMenuState currentMenuState = EMenuState.MainMenu;

    public List<MenuPanel> menus;
    MenuPanel currentMenu;
    MenuPanel currentPopup;

    [SerializeField]
    private SObject_Player mConfig_PlayerData = null;

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

    public void ActiveState(int iMenuState)
    {
        ActiveState((EMenuState)iMenuState);
    }
    public void ActiveState(EMenuState eMenuState)
    {
        currentMenuState = eMenuState;

        for (int i = 0; i < menus.Count; i++)
        {
            if(menus[i].menuState == eMenuState)
            {
                if(menus[i].mIsPopup)
                {
                    currentPopup = menus[i];
                    currentPopup.ActiveAlpha(true);
                    break;
                }

                if (currentMenu != null)
                    currentMenu.ActiveAlpha(false);
                if (currentPopup != null)
                    currentPopup.ActiveAlpha(false);

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
            case EMenuState.Account_Sessions_List:
                ActiveState(EMenuState.Account_Connection);
                //logout
                break;
            case EMenuState.Lobby_Session:
                ActiveState(EMenuState.Account_Sessions_List);
                break;
            case EMenuState.Lobby_Session_Password:
                ActiveState(EMenuState.Account_Sessions_List);
                break;
            case EMenuState.Lobby_Player_Character_Creation:
                ActiveState(EMenuState.Lobby_Session);
                break;
            case EMenuState.Lobby_Player_Character_Preview:
                ActiveState(EMenuState.Lobby_Session);
                //Disconnected
                break;
            case EMenuState.GameMaster_Mode:
                ActiveState(EMenuState.Account_Connection);
                //logout
                break;
            case EMenuState.GameMaster_New_Session:
                ActiveState(EMenuState.Account_Sessions_List);
                //logout
                break;
            case EMenuState.Adventure_Start_Session:
                ActiveState(EMenuState.MainMenu);
                //logout
                break;


            case EMenuState.Editor_Menu:
                ActiveState(EMenuState.Editor_Menu);
                break;
            case EMenuState.Editor_New_Map:
                ActiveState(EMenuState.Editor_Menu);
                break;
            case EMenuState.Editor_Save_Map:
                ActiveState(EMenuState.Editor_Menu);
                break;
            case EMenuState.Editor_Load_Map:
                ActiveState(EMenuState.Editor_Menu);
                break;

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
    Account_Sessions_List = 4,

    Lobby_Session = 10,
    Lobby_Player_Character_Creation = 11,
    Lobby_Player_Character_Preview = 12,
    Lobby_Session_Password = 13,

    GameMaster_Mode = 20,
    GameMaster_Editor = 21,
    GameMaster_New_Session = 22,

    Adventure_Start_Session = 30,

    Editor_Menu = 40,
    Editor_New_Map = 41,
    Editor_Load_Map = 42,
    Editor_Save_Map = 43,
}
