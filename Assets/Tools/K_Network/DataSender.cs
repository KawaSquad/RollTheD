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
            public static void SendPawnDestination(Vector3 destination)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_MOVE_PAWN);
                bufer.WriteFloat(destination.x);
                bufer.WriteFloat(destination.y);
                bufer.WriteFloat(destination.z);
                ClientTCP.SendData(bufer.ToArray());
                bufer.Dispose();
            }
        }
    }
}