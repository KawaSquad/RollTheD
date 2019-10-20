using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class PlayerController : TokenPawn
{
    [System.Serializable]
    public class PlayerController_Data
    {
        public int id_Character = 0;
        public int id_Token = 0;
        public string className = "novice";
    }
    public PlayerController_Data playerController_Data;

    public override void Initialize()
    {
        base.Initialize();

        if (serverData.pawnType != PawnPackets.P_Player)
        {
            Debug.LogError("try to instance pawn with wrong type");
            return;
        }

        playerController_Data = JsonUtility.FromJson<PlayerController_Data>(serverData.classParsed);

        if (playerController_Data.id_Token < offsetsTexture.Length)
            material.SetTextureOffset("_MainTex", offsetsTexture[playerController_Data.id_Token]);
    }
    public override string GetClassParsed()
    {
        return JsonUtility.ToJson(playerController_Data);
    }
}
