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

            C_PAWN_MOVE = 10,
            C_NEW_PAWN = 11,
            C_ASSIGN_PAWN = 12,
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
                bool isLocalClient = buffer.ReadBool();
                buffer.Dispose();

                NetworkManager.instance.InstantiatePlayerHandler(index, isLocalClient);
            }

            public static void HandleNewCharacter(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int indexSender = buffer.ReadInteger();
                int connectionID = buffer.ReadInteger();
                int ID_Charater = buffer.ReadInteger();
                buffer.Dispose();

                AdventureManager.Instance.CreateCharacter(connectionID, ID_Charater, false);
            }
            public static void HandleAssignCharacter(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int indexSender = buffer.ReadInteger();
                int ID_Charater = buffer.ReadInteger();
                int connectionID = buffer.ReadInteger();
                buffer.Dispose();

                Debug.Log("Assign : " + ID_Charater + " to : " + connectionID);
                //NetworkManager.instance.InstantiatePlayerHandler(index, isLocalClient);
            }

            public static void HandlePawnMove(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int index = buffer.ReadInteger();
                int id_Character = buffer.ReadInteger();
                float pos_x = buffer.ReadFloat();
                float pos_y = buffer.ReadFloat();
                float pos_z = buffer.ReadFloat();
                buffer.Dispose();

                NetworkManager.instance.Player_MovePawn(index, id_Character, new Vector3(pos_x,pos_y,pos_z));
            }

        }
    }
}