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
            public static void SendNewCharacter(int connectionID,int ID_Character)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_NEW_PAWN);
                bufer.WriteInteger(connectionID);
                bufer.WriteInteger(ID_Character);
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


            public static void SendPawnDestination(int id_Character, Vector3 destination)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_MOVE_PAWN);
                //bufer.WriteInteger(connectionID);
                bufer.WriteInteger(id_Character);

                bufer.WriteFloat(destination.x);
                bufer.WriteFloat(destination.y);
                bufer.WriteFloat(destination.z);
                ClientTCP.SendData(bufer.ToArray());
                bufer.Dispose();
            }
        }
    }
}