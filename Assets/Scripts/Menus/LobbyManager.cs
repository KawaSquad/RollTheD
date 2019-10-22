using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("Current session")]
    public Button_Character_Session sampleCharacter;
    public Transform listCharacterParent;

    [Header("New character")]
    public Dropdown dropClasses;
    public Dropdown dropRaces;

    [System.Serializable]
    public class JsonList
    {
        public List<string> content;
        public List<JsonClasses> classes;
        public List<JsonRaces> races;
    }
    [System.Serializable]
    public class JsonClasses
    {
        public string Class;
    }
    [System.Serializable]
    public class JsonRaces
    {
        public string Race;
    }

    [Header("Session data")]
    public SObject_Player sessionData;


    public void StartAsMaster()
    {
        if(sessionData.Game_Master)
        {
            MenuManager.Instance.ActiveState(EMenuState.Adventure_Start_Session);
        }
        else
        {
            Debug.Log("CHECK IF MASTER STARTED;");
        }
        // ACTIVE MAP
    }
    public void OpenNewCharcater()
    {
        Refresh_Classes_Races();
        MenuManager.Instance.ActiveState(EMenuState.Lobby_Player_Character_Creation);
    }
    public void CreateNewCharcater()
    {
        MenuManager.Instance.ActiveState(EMenuState.Lobby_Player_Character_Preview);
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
            Json_Content_Lobby contentAccount = JsonUtility.FromJson<Json_Content_Lobby>(requested.content);
            sessionData.Content_Lobby.characters = contentAccount.characters;

            for (int i = 0; i < contentAccount.characters.Count; i++)
            {
                Content_Lobby character = contentAccount.characters[i];
                Button_Character_Session bSession = Instantiate(sampleCharacter, listCharacterParent);
                string urlPP = DataBaseManager.DataBase+ "Sessions/" + sessionData.Master_Session + "/Pictures/" + character.Picture_Url;
                if (character.Picture_Url == "")
                    urlPP = "";
                bSession.SetCharacter(character.Name_Character, character.Class_Character, character.ID_Account.ToString(), urlPP);
            }

            LoadingScreen.StopLoading();
        }
        else
        {
            Debug.Log(requested.error);
        }
    }


    public void Refresh_Classes_Races()
    {
        dropClasses.ClearOptions();
        dropRaces.ClearOptions();

        if (DataBaseManager.Instance != null)
            DataBaseManager.Instance.ClassesRacesList(new DataBaseManager.OnRequestEnd(OnRequestClasses));
    }


    public void OnRequestClasses(JsonRequest requested)
    {
        if (requested.success == "true")
        {
            JsonList contentList = JsonUtility.FromJson<JsonList>(requested.content);
            
            List<Dropdown.OptionData> optionsClasses = new List<Dropdown.OptionData>();
            for (int i = 0; i < contentList.classes.Count; i++)
                optionsClasses.Add(new Dropdown.OptionData(contentList.classes[i].Class));
            dropClasses.AddOptions(optionsClasses);

            List<Dropdown.OptionData> optionsRaces = new List<Dropdown.OptionData>();
            for (int i = 0; i < contentList.races.Count; i++)
                optionsRaces.Add(new Dropdown.OptionData(contentList.races[i].Race));
            dropRaces.AddOptions(optionsRaces);
        }
        else
        {
            Debug.LogError(requested.error);
        }
    }
}
