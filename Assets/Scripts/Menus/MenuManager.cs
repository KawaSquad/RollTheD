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
    private EMenuState currentMenuState;

    public List<MenuPanel> menus;
    MenuPanel currentMenu;

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


    public void ActivePlayer()
    {
        ActiveState(EMenuState.Player_Connection);
    }
    public void ActiveGameMaster()
    {
        ActiveState(EMenuState.GameMaster_Mode);
    }
}

public enum EMenuState
{
    Unknow = 0,
    MainMenu = 1,

    Player_Connection = 10,
    Player_Account = 11,
    Player_Session = 12,
    Player_Character = 13,

    GameMaster_Mode = 20,
    GameMaster_Session = 21,
    GameMaster_Editor = 22,

}
