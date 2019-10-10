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
                float pos_x = buffer.ReadFloat();
                float pos_y = buffer.ReadFloat();
                float pos_z = buffer.ReadFloat();
                buffer.Dispose();
                
                //Console.WriteLine("Player : '{0}' - position : '{1}''{2}''{3}' ", connectionID, pos_x, pos_y, pos_z);

                buffer = new ByteBuffer();
                buffer.WriteFloat(pos_x);
                buffer.WriteFloat(pos_y);
                buffer.WriteFloat(pos_z);
                ClientManager.PawnMove(connectionID, buffer.ToArray());
                buffer.Dispose();
            }
        }
    }
}
