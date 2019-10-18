using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class Pawn : MonoBehaviour
        {
            [System.Serializable]
            public class Server_PawnData
            {
                public Guid server_Ref;
                public int ID_Handler;
                public Vector3 position;
                public Vector3 rotation;
                public Vector3 scale;
            }
            public Server_PawnData serverData;

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