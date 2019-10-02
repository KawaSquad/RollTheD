using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("Current session")]
    public Button_Character_Session sampleCharacter;
    public Transform listCharacterParent;

    [Header("Session data")]
    public SObject_Session sessionData;

    public void OpenNewCharcater()
    {
        MenuManager.Instance.ActiveState(EMenuState.GameMaster_New_Session);
    }
    public void Refresh_Characters_List()
    {
        Button_Character_Session[] charactersData = listCharacterParent.GetComponentsInChildren<Button_Character_Session>();
        for (int i = 0; i < charactersData.Length; i++)
        {
            Destroy(charactersData[i].gameObject);
        }

        if (DataBaseManager.Instance != null)
            DataBaseManager.Instance.CharacterList(sessionData.ID_Session , new DataBaseManager.OnRequestEnd(OnRequestCharacters));
    }
    public void OnRequestCharacters(JsonRequest requested)
    {
        if (requested.success == "true")
        {
            Json_Content_Session contentAccount = JsonUtility.FromJson<Json_Content_Session>(requested.content);

            for (int i = 0; i < contentAccount.sessions.Count; i++)
            {
                Button_Character_Session bSession = Instantiate(sampleCharacter, listCharacterParent);
                //bSession.SetSessionData(contentAccount.sessions[i]);
            }

            LoadingScreen.StopLoading();
        }
        else
        {
            Debug.Log(requested.error);
        }
    }

}
