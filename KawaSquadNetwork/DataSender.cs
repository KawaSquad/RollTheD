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
            S_CREATE_PLAYER = 2,
        }

        public static class DataSender
        {
            public static void SendWelcomeMessage(int connectionID)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_WELCOME_MESSAGE);
                buffer.WriteString("Welcome:");
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            public static void SendInstantiatePlayer(int index,int connectionID)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_CREATE_PLAYER);
                buffer.WriteInteger(index);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
        }
    }
}
