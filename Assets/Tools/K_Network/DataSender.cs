using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public enum ServerPackets
        {
            S_HELLO_SERVER = 1,

            S_MOVE_PAWN = 10,
            S_NEW_PAWN = 11,
            S_ASSIGN_PAWN = 12,
        }
        class DataSender
        {
            public static void SendHelloServer()
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_HELLO_SERVER);
                bufer.WriteString("I am connected");
                ClientTCP.SendData(bufer.ToArray());
                bufer.Dispose();
            }
            public static void SendNewCharacter(PlayerController.Server_PawnData data)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_NEW_PAWN);
                bufer.WriteInteger(data.ID_Handler);
                bufer.WriteInteger(data.ID_Character);
                bufer.WriteVector3(data.position);
                bufer.WriteVector3(data.rotation);
                bufer.WriteVector3(data.scale);
                ClientTCP.SendData(bufer.ToArray());
                bufer.Dispose();
            }
            public static void SendAssignCharacter(int ID_Character, int connectionID)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_ASSIGN_PAWN);
                bufer.WriteInteger(ID_Character);
                bufer.WriteInteger(connectionID);
                ClientTCP.SendData(bufer.ToArray());
                bufer.Dispose();
            }
            public static void SendPawnDestination(PlayerController.Server_PawnData data)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_MOVE_PAWN);

                bufer.WriteGuid(data.server_Ref);
//                bufer.WriteInteger(data.ID_Handler); 

                bufer.WriteVector3(data.position);
                bufer.WriteVector3(data.rotation);
                bufer.WriteVector3(data.scale);
                ClientTCP.SendData(bufer.ToArray());
                bufer.Dispose();
            }
        }
    }
}