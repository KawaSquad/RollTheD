using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Session : MonoBehaviour
{
    [SerializeField]
    private Text mSessionIndex = null;
    [SerializeField]
    private Text mSessionName = null;
    [SerializeField]
    private Text mSessionMaster = null;
    [SerializeField]
    private Text mSessionPlayers = null;
    [SerializeField]
    private Image mSessionLock = null;

    [SerializeField]
    private SObject_Player mSessionData = null;

    private Content_Session mSessionContent;

    public void SetSessionData(Content_Session sessionData)
    {
        mSessionContent = sessionData;

        mSessionIndex.text = mSessionContent.ID_Session.ToString();
        mSessionName.text = mSessionContent.Name_Session;
        mSessionMaster.text = mSessionContent.Master_Session;
        mSessionPlayers.text = /*mSessionContent.Number_Player + */ "-/" + mSessionContent.Number_Player_Max;
        mSessionLock.enabled = (mSessionContent.Password_Session != "");

        if (DataBaseManager.Instance != null)
            DataBaseManager.Instance.CharacterList(mSessionContent.ID_Session,new DataBaseManager.OnRequestEnd(OnRequestCountBySession),true);
    }
    public void OnRequestCountBySession(JsonRequest requested)
    {
        if (requested.success == "true")
        {
            Json_Content_Lobby lobby = JsonUtility.FromJson<Json_Content_Lobby>(requested.content);
            mSessionContent.Number_Player = lobby.characters.Count;
        }
        else
        {
            mSessionContent.Number_Player = 0;
        }
        mSessionPlayers.text = mSessionContent.Number_Player + "/" + mSessionContent.Number_Player_Max;
    }


    public void OnClick()
    {
        mSessionData.ID_Session = mSessionContent.ID_Session;
        mSessionData.Name_Session = mSessionContent.Name_Session;
        mSessionData.Master_Session = mSessionContent.Master_Session;
        mSessionData.Number_Player = mSessionContent.Number_Player;
        mSessionData.Number_Player_Max = mSessionContent.Number_Player_Max;
        mSessionData.Password_Session = mSessionContent.Password_Session;
        mSessionData.GM_Url = DataBaseManager.DataBase + "Sessions/"+ mSessionContent.Master_Session + "/";
        mSessionData.Maps = mSessionContent.Maps.Split(',');

        if (mSessionContent.Password_Session == "")//no password
        {
            MenuManager.Instance.ActiveState(EMenuState.Lobby_Session);
        }
        else
        {
            SessionManager.Instance.OpenSessionWithPassword();
        }
    }
}
