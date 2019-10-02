using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    [Header("Sessions list")]
    public Button bCreateSessionButton;
    public Button_Session buttonSessionSample;
    public Transform listSessionParent;

    [Header("New session")]
    public Inputfield_Form inpSessionName;
    public Inputfield_Form inpSessionMaster;
    [Header("Player data")]
    public SObject_Player playerData;
    public void Open_SessionsList()
    {
        bCreateSessionButton.gameObject.SetActive(playerData.Game_Master);
    }

    public void Refresh_Sessions()
    {
        Button_Session[] sessionsData = listSessionParent.GetComponentsInChildren<Button_Session>();
        for (int i = 0; i < sessionsData.Length; i++)
        {
            Destroy(sessionsData[i].gameObject);
        }

        if (DataBaseManager.Instance != null)
            DataBaseManager.Instance.Session_List(new DataBaseManager.OnRequestEnd(OnRequestSession));
    }

    public void OnRequestSession(JsonRequest requested)
    {
        if (requested.success == "true")
        {
            Json_Content_Session contentAccount = JsonUtility.FromJson<Json_Content_Session>(requested.content);

            for (int i = 0; i < contentAccount.sessions.Count; i++)
            {
                Button_Session bSession = Instantiate(buttonSessionSample, listSessionParent);
                bSession.SetSessionData(contentAccount.sessions[i]);
            }

            LoadingScreen.StopLoading();
        }
        else
        {
            Button_Session bSession = Instantiate(buttonSessionSample, listSessionParent);
            bSession.SetSessionData(new Content_Session() { ID_Session = 0, Name_Session = requested.error, Master_Session = "" });

            Debug.Log(requested.error);
        }
    }


    public void OpenNewSession()
    {
        MenuManager.Instance.ActiveState(EMenuState.GameMaster_New_Session);
    }

    public void ResetForms_Session()
    {
        inpSessionName.ResetField();
        inpSessionMaster.ResetField();
    }


    public void Create_Session()
    {
        bCreateSessionButton.gameObject.SetActive(false);
        Button_Session[] sessionsData = listSessionParent.GetComponentsInChildren<Button_Session>();
        for (int i = 0; i < sessionsData.Length; i++)
        {
            Destroy(sessionsData[i].gameObject);
        }

        if (DataBaseManager.Instance != null)
            DataBaseManager.Instance.CreateNewSession(inpSessionName.Content, inpSessionMaster.Content, "127.0.0.1", new DataBaseManager.OnRequestEnd(OnRequestSessionCreated));
    }

    public void OnRequestSessionCreated(JsonRequest requested)
    {
        if (requested.success == "true")
        {
            MenuManager.Instance.ActiveState(EMenuState.Player_Character);
            LoadingScreen.StopLoading();
        }
        else
        {
            Debug.Log(requested.error);
        }
    }

}
