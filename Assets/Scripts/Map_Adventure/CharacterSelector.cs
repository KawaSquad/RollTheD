using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class CharacterSelector : MonoBehaviour
{
    public int id_Character;

    public void OnClick()
    {
        if(AdventureManager.Instance != null)
        {
            if (PlayerHandle.LocalPlayerHandle != null)
                AdventureManager.Instance.CreateCharacter(PlayerHandle.LocalPlayerHandle.connectionID, id_Character, true);
            else
                Debug.LogError("error localPlayer");
        }
    }
}
