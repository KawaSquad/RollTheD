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
            S_WELCOME_MESSAGE = 1,
            S_CREATE_PLAYER = 2,
            S_PING_CLIENT = 3,

            S_PAWN_MOVE = 10,
            S_NEW_PAWN = 11,
            S_ASSIGN_PAWN = 12,
        }

        public static class DataSender
        {
            public static void SendWelcomeMessage(int connectionID)
            {
                if (connectionID == ServerTCP.PORT)//this is the server, no need to say welcome to the server
                    return;

                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_WELCOME_MESSAGE);
                buffer.WriteString("Welcome");
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();

                Debug.Log("send welcome to : " + connectionID);
            }
            public static void SendPingClient(int connectionID)
            {
                ByteBuffer bufer = new ByteBuffer();
                bufer.WriteInteger((int)ServerPackets.S_PING_CLIENT);
                int packageSize = 128;
                bufer.WriteInteger(packageSize);//PackageSize
                bufer.WriteBytes(new byte[packageSize]);
                ClientManager.SendDataTo(connectionID, bufer.ToArray());
                bufer.Dispose();
                Debug.Log("send ping to : " + connectionID);
            }


            #region PlayerHandle
            public static void SendInstantiatePlayer(int index, int connectionID,bool isLocalClient = false)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_CREATE_PLAYER);
                buffer.WriteInteger(index);
                buffer.WriteBool(isLocalClient);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
                Debug.Log("send new player to : " + connectionID);
            }
            #endregion

            #region Pawn
            public static void SendNewPawn(int connectionID, Pawn pawn)//Index how move, Conn send to
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_NEW_PAWN);
                buffer.WriteGuid(pawn.server_Ref);
                buffer.WriteInteger(pawn.ID_Hanlder);
                //buffer.WriteInteger(pawn.ID_Character);
                buffer.WriteVector3(pawn.transform.position);
                buffer.WriteVector3(pawn.transform.rotation);
                buffer.WriteVector3(pawn.transform.scale);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
                Debug.Log("send new pawn" + pawn.server_Ref + "on : " + connectionID);
            }
            public static void SendAssignPawn(int index, int connectionID, byte[] data)//Index how move, Conn send to
            {
                /*
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_ASSIGN_PAWN);
                buffer.WriteInteger(index);
                buffer.WriteBytes(data);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
                 */
                Debug.Log("assign pawn" + index + "on : " + connectionID);
            }
            public static void SendPawnMove(int connectionID, Guid server_Ref, Transform pawnTransform)//Index how move, Conn send to
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_PAWN_MOVE);
                buffer.WriteGuid(server_Ref);
//                buffer.WriteInteger(handler);
                buffer.WriteVector3(pawnTransform.position);
                buffer.WriteVector3(pawnTransform.rotation);
                buffer.WriteVector3(pawnTransform.scale);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
                Debug.Log("move pawn" + server_Ref + "on : " + connectionID);
            }
            #endregion
        }
    }
}
