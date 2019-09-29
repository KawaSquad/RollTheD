using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player")]
    public InputField inpPlayerName;
    public InputField inpPlayerPassword;

    [Header("New Player Account")]
    public InputField inpNewPlayerName;
    public InputField inpNewPlayerPassword;

    #region Player account
    public void CheckPlayerAccount()
    {
        if (inpPlayerName.text != "" && inpPlayerName.text != "")
        {
            MenuManager.Instance.ActiveState(EMenuState.Player_Session);
        }
    }
    #endregion

    #region New Player
    public void OpenNewPlayer()
    {
        MenuManager.Instance.ActiveState(EMenuState.Player_Account);
    }
    public void CreateNewPlayer()
    {
        if(inpNewPlayerName.text != "" && inpNewPlayerPassword.text != "")
        {
            MenuManager.Instance.ActiveState(EMenuState.Player_Connection);
        }
    }
    #endregion
}
