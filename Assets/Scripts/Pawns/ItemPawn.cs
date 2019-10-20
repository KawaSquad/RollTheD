using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class ItemPawn : TokenPawn
{
    [System.Serializable]
    public class ItemPawn_Data
    {
        public int id_Token = 0;
    }
    [HideInInspector]
    public ItemPawn_Data item_Data;

    public override void Initialize()
    {
        base.Initialize();

        if (serverData.pawnType != PawnPackets.P_Items)
        {
            Debug.LogError("try to instance pawn with wrong type");
            return;
        }

        item_Data = JsonUtility.FromJson<ItemPawn_Data>(serverData.classParsed);
    }
    public override string GetClassParsed()
    {
        return JsonUtility.ToJson(item_Data);
    }
}
