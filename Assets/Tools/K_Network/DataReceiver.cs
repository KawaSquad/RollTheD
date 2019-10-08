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
            C_WELCOME_MESSAGE = 1,
            C_CREATE_PLAYER = 2,
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
                DataSender.SendHelloServer();
            }

            public static void HandleInstantiatePlayer(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int index = buffer.ReadInteger();
                buffer.Dispose();

                NetworkManager.instance.InstantiatePlayer(index);
            }

            public static void HandlePawnMove(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int index = buffer.ReadInteger();
                buffer.Dispose();

                NetworkManager.instance.InstantiatePlayer(index);
            }
        }
    }
}