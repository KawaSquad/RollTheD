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
            C_PING_CLIENT = 3,

            C_PAWN_MOVE = 10,
            C_NEW_PAWN = 11,
            C_ASSIGN_PAWN = 12,
            C_DELETE_PAWN = 13,

            C_LOAD_MAP = 20,
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
            public static void HandlePingServer(byte[] data)
            {
                /*
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int packageSize = buffer.ReadInteger();
                byte[] package = buffer.ReadBytes(packageSize);
                buffer.Dispose();
                 */
                NetworkManager.HanldePing();
            }

            public static void HandleNewPawn(byte[] data)
            {
                Pawn.Server_PawnData dataPawn = new Pawn.Server_PawnData();

                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                dataPawn.server_Ref = buffer.ReadGuid();
                dataPawn.ID_Handler = buffer.ReadInteger();

                dataPawn.position = buffer.ReadVector3();
                dataPawn.rotation = buffer.ReadVector3();
                dataPawn.scale = buffer.ReadVector3();

                dataPawn.pawnType = (Pawn.PawnPackets)buffer.ReadInteger();
                dataPawn.classParsed = buffer.ReadString();

                buffer.Dispose();

                NetworkManager.instance.InstantiatePawn(dataPawn);
            }
            public static void HandleAssignPawn(byte[] data)
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
                Guid server_Ref = buffer.ReadGuid();
//                int handler = buffer.ReadInteger();
                Vector3 position = buffer.ReadVector3();
                Vector3 rotation = buffer.ReadVector3();
                Vector3 scale = buffer.ReadVector3();

                buffer.Dispose();

                NetworkManager.instance.Player_MovePawn(server_Ref, position, rotation, scale);
            }

            public static void HandleDeletePawn(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                Guid server_Ref = buffer.ReadGuid();
                buffer.Dispose();

                NetworkManager.instance.Player_DeletePawn(server_Ref);
            }
            public static void HandleLoadMap(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                string mapPath = buffer.ReadString();
                buffer.Dispose();

                if (MapLoader.instance != null)
                    MapLoader.instance.LoadMap(mapPath, false);
                else
                    Debug.LogError("instance is missing");
            }
        }
    }
}