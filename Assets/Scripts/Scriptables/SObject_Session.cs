using UnityEngine;

[CreateAssetMenu(fileName = "Session_Data", menuName = "KawaSquad/SessionData")]
public class SObject_Session : ScriptableObject
{
    //Session
    [Header("Session")]
    public int ID_Session = 0;
    public string Name_Session = "name";
    public string Master_Session = "name";

}
