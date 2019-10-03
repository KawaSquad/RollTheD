using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Session : MonoBehaviour
{
    [SerializeField]
    private Text mSessionIndex;
    [SerializeField]
    private Text mSessionName;
    [SerializeField]
    private Text mSessionMaster;
    [SerializeField]
    private Text mSessionPlayers;
    [SerializeField]
    private Image mSessionLock;

    [SerializeField]
    private SObject_Player mSessionData;

    private Content_Session mSessionContent;

    public void SetSessionData(Content_Session sessionData)
    {
        mSessionContent = sessionData;

        mSessionIndex.text = mSessionContent.ID_Session.ToString();
        mSessionName.text = mSessionContent.Name_Session;
        mSessionMaster.text = mSessionContent.Master_Session;
        mSessionPlayers.text = mSessionContent.Number_Player + "/" + mSessionContent.Number_Player_Max;
        mSessionLock.enabled = (mSessionContent.Password_Session != "");
    }

    public void OnClick()
    {
        mSessionData.ID_Session = mSessionContent.ID_Session;
        mSessionData.Name_Session = mSessionContent.Name_Session;
        mSessionData.Master_Session = mSessionContent.Master_Session;
        mSessionData.Number_Player = mSessionContent.Number_Player;
        mSessionData.Number_Player_Max = mSessionContent.Number_Player_Max;
        mSessionData.Password_Session = mSessionContent.Password_Session;
        mSessionData.Save_Url = mSessionContent.Save_Url;

        if (mSessionContent.Password_Session == "")//no password
        {
            MenuManager.Instance.ActiveState(EMenuState.Lobby_Session);
        }
        else
        {
            SessionManager.Instance.OpenSessionWithPassword();
            Debug.Log("OPEN POPUP PASSWORD");
        }
    }
}
