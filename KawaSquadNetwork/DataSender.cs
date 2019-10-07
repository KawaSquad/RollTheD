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
            S_WELCOME_MESSAGE = 1,
        }

        public static class DataSender
        {
            public static void SendWelcomeMessage(int connectionID)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_WELCOME_MESSAGE);
                buffer.WriteString("HELLO STEVEN");
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
        }
    }
}
