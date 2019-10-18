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
            C_HELLO_SERVER = 1,
            C_PING_SERVER = 2,

            C_MOVE_PAWN = 10,
            C_NEW_PAWN = 11,
            C_ASSIGN_PAWN = 12,
            C_DELETE_PAWN = 13,

            C_LOAD_MAP = 20,
        }

        public class DataReceiver
        {
            #region Server
            public static void HandleHelloServer(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                string msg = buffer.ReadString();
                buffer.Dispose();
                Debug.Log(msg,true);
            }
            public static void HandlePingServer(int connectionID, byte[] data)
            {
                /*
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int packageSize = buffer.ReadInteger();
                byte[] package = buffer.ReadBytes(packageSize);
                buffer.Dispose();
                 */
                DataSender.SendPingClient(connectionID);
            }
            #endregion

            #region Pawn
            public static void HandleNewPawn(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int handlerID = buffer.ReadInteger();
//                int ID_Character = buffer.ReadInteger();
                Vector3 position = buffer.ReadVector3();
                Vector3 rotation = buffer.ReadVector3();
                Vector3 scale = buffer.ReadVector3();
                buffer.Dispose();

                //Debug.Log("New character : '{0}' from '{1}'", ID_Character, connectionID);

                Pawn newPawn = new Pawn();
                newPawn.server_Ref = Guid.NewGuid();
                newPawn.ID_Hanlder = handlerID;
//                newPawn.ID_Character = ID_Character;
                newPawn.transform = new Transform(position, rotation, scale);
                ClientManager.NewPawn(connectionID, newPawn);
            }
            public static void HandleAssignPawn(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int ID_Character = buffer.ReadInteger();
                int handlerID = buffer.ReadInteger();
                buffer.Dispose();

                //Debug.Log("Player : '{0}' - position : '{1}''{2}''{3}' ", connectionID, pos_x, pos_y, pos_z);

                buffer = new ByteBuffer();
                buffer.WriteInteger(ID_Character);
                buffer.WriteInteger(handlerID);
                ClientManager.AssignPawn(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            public static void HandlePawnMove(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                Guid server_Ref = buffer.ReadGuid();
//                int ID_Handler = buffer.ReadInteger();
                Vector3 position = buffer.ReadVector3();
                Vector3 rotation = buffer.ReadVector3();
                Vector3 scale = buffer.ReadVector3();
                buffer.Dispose();

                //Debug.Log("Character : '{0}' from '{1}' - position : '{2}''{3}''{4}' ", id_Character, connectionID, pos_x, pos_y, pos_z);

                Transform pawnTransform = new Transform(position, rotation, scale );
                ClientManager.PawnMove(connectionID, server_Ref, pawnTransform);
            }
            public static void HandlePawnDelete(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                Guid server_Ref = buffer.ReadGuid();
                buffer.Dispose();

                ClientManager.DeletePawn(server_Ref);
            }
            #endregion


            #region Map
            public static void HandleLoadMap(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                string mapPath = buffer.ReadString();
                buffer.Dispose();
                ClientManager.LoadMap(connectionID, mapPath);
            }
            #endregion

        }
    }
}
