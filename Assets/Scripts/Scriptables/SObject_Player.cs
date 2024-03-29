﻿using UnityEngine;

[CreateAssetMenu(fileName = "Player_Data", menuName = "KawaSquad/PlayerData")]
public class SObject_Player : ScriptableObject
{
    //ACCOUNT
    [Header("Account")]
    public int ID_Account = 0;
    public string Name_Account = "name";
    public string Password_Account = "psw";
    public bool Game_Master = false;
 
    //Character
    [Header("Character")]
    public int ID_Character = 0;
    public string Name_Character = "name";
    public string Class_Character = "class";

    //Session
    [Header("Session")]
    public int ID_Session = 0;
    public string Name_Session = "name";
    public string Master_Session = "name";
    public int Number_Player = 0;
    public int Number_Player_Max = 8;
    public string Password_Session = "psw";
    public string IP_Session = "127.0.0.1";
    public int Port_Session = 5557;
    public string GM_Url = "https://steven-sternberger.be/RollTheD/Sessions/styven184/";
    public string[] Maps = { "Map_01.json","Map_02.json" };

    public Json_Content_Lobby Content_Lobby;
}
