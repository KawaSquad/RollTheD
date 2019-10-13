using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    private static SessionManager instance;
    public static SessionManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("Instance not already set");
            return instance;
        }
    }

    [Header("Sessions list")]
    public Button bCreateSessionButton;
    public Button_Session buttonSessionSample;
    public Transform listSessionParent;

    [Header("New session")]
    public Inputfield_Form inpSessionName;
    public Inputfield_Form inpSessionMaster;

    [Header("Protected session")]
    public Text textSessionName;
    public Inputfield_Form inpSessionPassword;

    [Header("Player data")]
    public SObject_Player sessionData;

    private void Awake()
    {
        if(instance != null)
            Debug.LogError("Instance already assigned");
        instance = this;
    }

    public void Open_SessionsList()
    {
        bCreateSessionButton.gameObject.SetActive(sessionData.Game_Master);
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
            MenuManager.Instance.ActiveState(EMenuState.Account_Sessions_List);
            LoadingScreen.StopLoading();
        }
        else
        {
            inpSessionMaster.DisplayError(requested.error);
            Debug.Log(requested.error);
        }
    }

    public void OpenSessionWithPassword()
    {
        MenuManager.Instance.ActiveState(EMenuState.Lobby_Session_Password);
        textSessionName.text = sessionData.Name_Session;
        inpSessionPassword.ResetField();
    }

    public void CheckSessionWithPassword()
    {
        if (inpSessionPassword.Validate())
        {
            if (inpSessionPassword.Content.GetHashCode().ToString() == sessionData.Password_Session)
            {
                MenuManager.Instance.ActiveState(EMenuState.Lobby_Session);
            }
            else
            {
                inpSessionPassword.DisplayError("Wrong password");
            }
        }
        else
        {
            inpSessionPassword.DisplayError("Password required");
        }
    }
}
