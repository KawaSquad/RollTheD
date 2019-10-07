using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        public enum ClientPackets
        {
            C_HELLO_SERVER = 1,
        }

        public class DataReceiver
        {
            public static void HandleHelloServer(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                string msg = buffer.ReadString();
                buffer.Dispose();
                Console.WriteLine(msg);
            }
        }
    }
}
