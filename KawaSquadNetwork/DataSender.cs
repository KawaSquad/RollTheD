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

            S_PAWN_MOVE = 10,
            S_NEW_PAWN = 11,
            S_ASSIGN_PAWN = 12,
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
            #region PlayerHandle
            public static void SendInstantiatePlayer(int index, int connectionID,bool isLocalClient = false)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_CREATE_PLAYER);
                buffer.WriteInteger(index);
                buffer.WriteBool(isLocalClient);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            #endregion

            #region Pawn
            public static void SendPawnMove(int index, int connectionID, Transform pawnTransform)//Index how move, Conn send to
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_PAWN_MOVE);
                buffer.WriteInteger(index);
                buffer.WriteVector3(pawnTransform.position);
                buffer.WriteVector3(pawnTransform.rotation);
                buffer.WriteVector3(pawnTransform.scale);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            public static void SendNewPawn(int connectionID, Pawn pawn)//Index how move, Conn send to
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_NEW_PAWN);
                buffer.WriteInteger(pawn.ID_Hanlder);
                buffer.WriteInteger(pawn.ID_Character);
                buffer.WriteVector3(pawn.transform.position);
                buffer.WriteVector3(pawn.transform.rotation);
                buffer.WriteVector3(pawn.transform.scale);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            public static void SendAssignPawn(int index, int connectionID, byte[] data)//Index how move, Conn send to
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ServerPackets.S_ASSIGN_PAWN);
                buffer.WriteInteger(index);
                buffer.WriteBytes(data);
                ClientManager.SendDataTo(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            #endregion
        }
    }
}
