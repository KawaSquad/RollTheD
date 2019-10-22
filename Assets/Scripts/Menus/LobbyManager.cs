using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("Session data")]
    public SObject_Player sessionData;

    [Header("Current session")]
    public Button_Character_Session sampleCharacter;
    public Transform listCharacterParent;

    [Header("New character infos")]
    //character
    public Inputfield_Form inpCharacterName;
    public Inputfield_Form inpLevel;
    public Inputfield_Form inpHp, inpHpMax;
    public Inputfield_Form inpGold;

    //Classes
    [Header("New character class & race")]
    public Dropdown dropClasses;
    public Dropdown dropRaces;

    //ID_ForeignKey in the session data

    //ID_Token TODO

    //Stats
    [Header("New character stats")]
    public Inputfield_Form inpStat_Str;
    public Inputfield_Form inpStat_Dex;
    public Inputfield_Form inpStat_Int;
    public Inputfield_Form inpStat_Con;
    public Inputfield_Form inpStat_Wis;
    public Inputfield_Form inpStat_Cha;

    public class CharacterFormat
    {
        public string characterName;
        public int level;
        public int hp;
        public int hp_max;
        public int gold;

        public string className;
        public string raceName;

        public int id_account;
        public int id_session;

        public string url_picture = "null";
        public string url_token= "null";

        public int stat_str;
        public int stat_dex;
        public int stat_int;
        public int stat_con;
        public int stat_wis;
        public int stat_cha;
    }


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

    public void StartAsMaster()
    {
        if (sessionData.Game_Master)
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
        if (CharacterFormIsValide(out CharacterFormat characterFormat))
        {
            WWWForm characterForm = new WWWForm();

            //Character
            characterForm.AddField("character_name_Post", characterFormat.characterName);
		    characterForm.AddField("level_Post", characterFormat.level);
		    characterForm.AddField("hp_Post", characterFormat.hp); 
		    characterForm.AddField("hp_max_Post", characterFormat.hp_max); 
		    characterForm.AddField("gold_Post", characterFormat.gold);

            //Foreign key
            characterForm.AddField("class_Post", characterFormat.className);
            characterForm.AddField("race_Post", characterFormat.raceName);

		    characterForm.AddField("account_Post", sessionData.ID_Account);
		    characterForm.AddField("session_Post", sessionData.ID_Session);

		    //characterForm.AddField("token_Post", 0);
		    
		    //URl
		    characterForm.AddField("picture_url_Post", characterFormat.url_picture); 
		    characterForm.AddField("token_url_Post", characterFormat.url_token);

		    //Stats
		    characterForm.AddField("stat_str_Post", characterFormat.stat_str);
		    characterForm.AddField("stat_dex_Post", characterFormat.stat_dex);
		    characterForm.AddField("stat_int_Post", characterFormat.stat_int);
		    characterForm.AddField("stat_con_Post", characterFormat.stat_con);
		    characterForm.AddField("stat_wis_Post", characterFormat.stat_wis);
		    characterForm.AddField("stat_cha_Post", characterFormat.stat_cha);

            if (DataBaseManager.Instance != null)
                DataBaseManager.Instance.CreateNewCharacter(characterForm, new DataBaseManager.OnRequestEnd(OnRequestCharacterCreated));
        }
    }
    public void OnRequestCharacterCreated(JsonRequest requested)
    {
        if(requested.content == "true")
            MenuManager.Instance.ActiveState(EMenuState.Lobby_Session);
    }

    public bool CharacterFormIsValide(out CharacterFormat characterFormat)
    {
        characterFormat = new CharacterFormat();


        //Character name
        if (inpCharacterName.Validate())
            characterFormat.characterName = inpCharacterName.Content;
        else
        {
            inpCharacterName.DisplayError("Invalid field");
            return false;
        }

        //level
        if (inpLevel.Validate())
            characterFormat.level = int.Parse(inpLevel.Content);
        else
        {
            inpLevel.DisplayError("Invalid field");
            return false;
        }

        //HP
        if (inpHp.Validate())
            characterFormat.hp = int.Parse(inpHp.Content);
        else
        {
            inpHp.DisplayError("Invalid field");
            return false;
        }

        //HP_MAX
        if (inpHpMax.Validate())
            characterFormat.hp_max = int.Parse(inpHpMax.Content);
        else
        {
            inpHpMax.DisplayError("Invalid field");
            return false;
        }

        //HP_MAX
        if (inpGold.Validate())
            characterFormat.hp_max = int.Parse(inpGold.Content);
        else
        {
            inpGold.DisplayError("Invalid field");
            return false;
        }

        //Class && race name
        characterFormat.className = dropClasses.options[dropClasses.value].text;
        characterFormat.raceName = dropRaces.options[dropRaces.value].text;


        //STATS_STR
        if (inpStat_Str.Validate())
            characterFormat.stat_str = int.Parse(inpStat_Str.Content);
        else
        {
            inpStat_Str.DisplayError("Invalid field");
            return false;
        }

        //STATS_DEX
        if (inpStat_Dex.Validate())
            characterFormat.stat_dex = int.Parse(inpStat_Dex.Content);
        else
        {
            inpStat_Dex.DisplayError("Invalid field");
            return false;
        }

        //STATS_INT
        if (inpStat_Int.Validate())
            characterFormat.stat_int = int.Parse(inpStat_Int.Content);
        else
        {
            inpStat_Int.DisplayError("Invalid field");
            return false;
        }

        //STATS_CON
        if (inpStat_Con.Validate())
            characterFormat.stat_con = int.Parse(inpStat_Con.Content);
        else
        {
            inpStat_Con.DisplayError("Invalid field");
            return false;
        }

        //STATS_WIS
        if (inpStat_Wis.Validate())
            characterFormat.stat_wis = int.Parse(inpStat_Wis.Content);
        else
        {
            inpStat_Wis.DisplayError("Invalid field");
            return false;
        }

        //STATS_CHA
        if (inpStat_Cha.Validate())
            characterFormat.stat_cha = int.Parse(inpStat_Cha.Content);
        else
        {
            inpStat_Cha.DisplayError("Invalid field");
            return false;
        }

        return true;
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
