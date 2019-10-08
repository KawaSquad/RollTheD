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
        public enum ClientPackets
        {
            S_WELCOME_MESSAGE = 1,
            S_CREATE_PLAYER = 2,
        }

        static class DataReceiver
        {
            public static void HandleWelcomeMessage(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                string msg = buffer.ReadString();
                buffer.Dispose();

                Debug.Log(msg);
            }
        }
    }
}