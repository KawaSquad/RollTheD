using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public static void HandleHelloServer(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                string msg = buffer.ReadString();
                buffer.Dispose();
                Console.WriteLine(msg);
            }

            public static void HandlePawnMove(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int id_Character = buffer.ReadInteger();
                float pos_x = buffer.ReadFloat();
                float pos_y = buffer.ReadFloat();
                float pos_z = buffer.ReadFloat();
                buffer.Dispose();

                Console.WriteLine("Character : '{0}' from '{1}' - position : '{2}''{3}''{4}' ", id_Character, connectionID, pos_x, pos_y, pos_z);

                buffer = new ByteBuffer();
                buffer.WriteInteger(id_Character);
                buffer.WriteFloat(pos_x);
                buffer.WriteFloat(pos_y);
                buffer.WriteFloat(pos_z);
                ClientManager.PawnMove(connectionID, buffer.ToArray());
                buffer.Dispose();
            }

            public static void HandleNewPawn(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                int handlerID = buffer.ReadInteger();
                int ID_Character = buffer.ReadInteger();
                buffer.Dispose();

                Console.WriteLine("New character : '{0}' from '{1}'", ID_Character, connectionID);

                buffer = new ByteBuffer();
                buffer.WriteInteger(handlerID);
                buffer.WriteInteger(ID_Character);
                ClientManager.NewPawn(connectionID, buffer.ToArray());
                buffer.Dispose();
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
        }
    }
}
