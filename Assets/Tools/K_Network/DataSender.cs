using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        public enum ServerPackets
        {
            S_HELLO_SERVER = 1,
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
        }
    }
}