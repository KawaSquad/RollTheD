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

    private Content_Session mSessionData;

    public void SetSessionData(Content_Session sessionData)
    {
        mSessionData = sessionData;

        mSessionIndex.text = mSessionData.ID_Session.ToString();
        mSessionName.text = mSessionData.Name_Session;
        mSessionMaster.text = mSessionData.Master_Session;
    }

    public void OnClick()
    {
        Debug.Log("OPEN Session " + mSessionData.Name_Session);
    }
}
