using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class EnemyController : TokenPawn
{
    [System.Serializable]
    public class EnemyController_Data
    {
        public int id_Token = 0;
    }
    [HideInInspector]
    public EnemyController_Data ennemyController_Data;

    public override void Initialize()
    {
        base.Initialize();

        if (serverData.pawnType != PawnPackets.P_Ennemy)
        {
            Debug.LogError("try to instance pawn with wrong type");
            return;
        }

        ennemyController_Data = JsonUtility.FromJson<EnemyController_Data>(serverData.classParsed);

        if (ennemyController_Data.id_Token < offsetsTexture.Length)
            material.SetTextureOffset("_MainTex", offsetsTexture[ennemyController_Data.id_Token]);
    }
    public override string GetClassParsed()
    {
        return JsonUtility.ToJson(ennemyController_Data);
    }
}
