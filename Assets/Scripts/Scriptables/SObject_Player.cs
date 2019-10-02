using UnityEngine;

[CreateAssetMenu(fileName = "Player_Data", menuName = "KawaSquad/PlayerData")]
public class SObject_Player : ScriptableObject
{
    //ACCOUNT
    [Header("Account")]
    public int ID_Account = 0;
    public string Name_Account = "name";
    public string Password_Account = "pswd";
    public bool Game_Master = false;
 
    //Character
    [Header("Character")]
    public int ID_Character = 0;
    public string Name_Character = "name";
    public string Class_Character = "class";
}
