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

            C_MOVE_PAWN = 10,
            C_NEW_PAWN = 11,
            C_ASSIGN_PAWN = 12,
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
                Console.WriteLine(msg);
            }
            #endregion

            #region Pawn
            public static void HandlePawnMove(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int id_Character = buffer.ReadInteger();
                Vector3 position = buffer.ReadVector3();
                Vector3 rotation = buffer.ReadVector3();
                Vector3 scale = buffer.ReadVector3();
                buffer.Dispose();

                //Console.WriteLine("Character : '{0}' from '{1}' - position : '{2}''{3}''{4}' ", id_Character, connectionID, pos_x, pos_y, pos_z);

                Transform pawnTransform = new Transform(position, rotation, scale );
                ClientManager.PawnMove(connectionID, pawnTransform);
            }
            public static void HandleNewPawn(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int handlerID = buffer.ReadInteger();
                int ID_Character = buffer.ReadInteger();
                Vector3 position = buffer.ReadVector3();
                Vector3 rotation = buffer.ReadVector3();
                Vector3 scale = buffer.ReadVector3();
                buffer.Dispose();

                Console.WriteLine("New character : '{0}' from '{1}'", ID_Character, connectionID);

                Pawn newPawn = new Pawn();
                newPawn.ID_Hanlder= handlerID;
                newPawn.ID_Character = ID_Character;
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

                //Console.WriteLine("Player : '{0}' - position : '{1}''{2}''{3}' ", connectionID, pos_x, pos_y, pos_z);

                buffer = new ByteBuffer();
                buffer.WriteInteger(ID_Character);
                buffer.WriteInteger(handlerID);
                ClientManager.AssignPawn(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
            #endregion
        }
    }
}
