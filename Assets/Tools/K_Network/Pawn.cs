﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class Pawn : MonoBehaviour
        {
            public enum PawnPackets
                {
                    P_Base = 1,

                    P_Player = 10,
                    P_Ennemy = 11,
                    P_Items = 12,
                }

            [System.Serializable]
            public class Server_PawnData
            {
                public Guid server_Ref;
                public int ID_Handler;

                public Vector3 position;
                public Vector3 rotation;
                public Vector3 scale;

                public PawnPackets pawnType;
                public string classParsed;
            }

            public PawnPackets pawnType = PawnPackets.P_Base;
            [HideInInspector]
            public Server_PawnData serverData;

            private void Start()
            {
                Initialize();
            }

            public virtual void Initialize()
            {

            }
            public virtual string GetClassParsed()
            {
                return "";
            }

            public void MovePawn(Vector3 position, Vector3 rotation, Vector3 scale)
            {
                serverData.position = position;
                serverData.rotation = rotation;
                serverData.scale = scale;

                DataSender.SendPawnDestination(serverData);
                SetPosition(position, rotation, scale);
            }
            public void SetPosition(Vector3 destination, Vector3 orientation, Vector3 scale)
            {
                Transform controller = this.transform;
                destination.y = controller.position.y;
                controller.position = destination;
                controller.eulerAngles = orientation;
                controller.localScale = scale;
            }
            public void DestroyPawn()
            {
                DataSender.SendPawnDestruct(serverData);
            }
        }
    }
}