using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterManager : MonoBehaviour
{
    /*
    [Header("Player")]
    public InputField inpPlayerName;
    public InputField inpPlayerPassword;

    [Header("New Player Account")]
    public InputField inpNewPlayerName;
    public InputField inpNewPlayerPassword;
     */

    #region Session
    //public void OpenSession()
    //{
    //    MenuManager.Instance.ActiveState(EMenuState.GameMaster_Session);
    //}
    #endregion

    #region Map editor
    public void OpenMapEditor()
    {
        Debug.Log("Map editor : coming soon");
        MenuManager.Instance.ActiveState(EMenuState.GameMaster_Editor);
        //MenuManager.Instance.ActiveState(EMenuState.Player_Account);
    }
    #endregion
}
