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
    //[SerializeField]
    //private Text mSessionIP;

    [SerializeField]
    private SObject_Session mSessionData;

    private Content_Session mSessionContent;

    public void SetSessionData(Content_Session sessionData)
    {
        mSessionContent = sessionData;

        mSessionIndex.text = mSessionContent.ID_Session.ToString();
        mSessionName.text = mSessionContent.Name_Session;
        mSessionMaster.text = mSessionContent.Master_Session;
    }

    public void OnClick()
    {
        mSessionData.ID_Session = mSessionContent.ID_Session;
        mSessionData.Name_Session = mSessionContent.Name_Session;
        mSessionData.Master_Session = mSessionContent.Master_Session;

        MenuManager.Instance.ActiveState(EMenuState.Lobby_Session);
        Debug.Log("OPEN Session " + mSessionData.Name_Session);
    }
}
