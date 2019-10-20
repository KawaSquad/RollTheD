using System;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class PlayerHandle : MonoBehaviour
        {
            public static PlayerHandle LocalPlayerHandle;

            private List<Pawn> pawns = null;
            private bool isLocalClient = false;

            [HideInInspector]
            public int connectionID = 0;

            private Vector3 currentPosition = Vector3.zero;

            private void Start()
            {
                pawns = new List<Pawn>();
            }

            public void SetPlayerHandle(int connectionID, bool isLocalClient)
            {
                this.connectionID = connectionID;
                this.name = "Player_" + connectionID;
                this.isLocalClient = isLocalClient;

                if (isLocalClient)
                    LocalPlayerHandle = this;
            }

            public void AssignedPawn(Pawn newPawn)
            {
                if (pawns == null)
                    pawns = new List<Pawn>();

                newPawn.transform.parent = this.transform;
                pawns.Add(newPawn);
            }
            public void RemovePawn(Pawn existingPawn)
            {
                if (pawns != null && pawns.Contains(existingPawn))
                {
                    pawns.Remove(existingPawn);
                    existingPawn.transform.parent = null;
                }
            }
        }
    }
}