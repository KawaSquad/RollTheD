using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        public class ServerHandleData
        {
            public delegate void Packet(int connectionID,byte[] data);

            public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

            public static void InitializePackets()
            {
                packets.Add((int)ClientPackets.C_HELLO_SERVER, DataReceiver.HandleHelloServer);
                packets.Add((int)ClientPackets.C_PING_SERVER, DataReceiver.HandlePingServer);

                packets.Add((int)ClientPackets.C_MOVE_PAWN, DataReceiver.HandlePawnMove);
                packets.Add((int)ClientPackets.C_NEW_PAWN, DataReceiver.HandleNewPawn);
                packets.Add((int)ClientPackets.C_ASSIGN_PAWN, DataReceiver.HandleAssignPawn);
                packets.Add((int)ClientPackets.C_DELETE_PAWN, DataReceiver.HandlePawnDelete);
            }
            public static void HandleData(int connectionID, byte[] data)
            {
                byte[] buffer = (byte[])data.Clone();
                int pLenght = 0;

                if (ClientManager.clients[connectionID].buffer == null)
                    ClientManager.clients[connectionID].buffer = new ByteBuffer();

                ClientManager.clients[connectionID].buffer.WriteBytes(buffer);
                if (ClientManager.clients[connectionID].buffer.Count == 0)
                {
                    ClientManager.clients[connectionID].buffer.Clear();
                    return;
                }

                if (ClientManager.clients[connectionID].buffer.Lenght >= 4)
                {
                    pLenght = ClientManager.clients[connectionID].buffer.ReadInteger(false);
                    if (pLenght <= 0)
                    {
                        ClientManager.clients[connectionID].buffer.Clear();
                        return;
                    }
                }

                while (pLenght > 0 && pLenght <= ClientManager.clients[connectionID].buffer.Lenght - 4)
                {
                    if (pLenght <= ClientManager.clients[connectionID].buffer.Lenght - 4)
                    {
                        ClientManager.clients[connectionID].buffer.ReadInteger();
                        data = ClientManager.clients[connectionID].buffer.ReadBytes(pLenght);
                        HandleDataPackets(connectionID, data);
                    }
                    pLenght = 0;

                    if (ClientManager.clients[connectionID].buffer.Lenght >= 4)
                    {
                        pLenght = ClientManager.clients[connectionID].buffer.ReadInteger(false);
                        if (pLenght <= 0)
                        {
                            ClientManager.clients[connectionID].buffer.Clear();
                            return;
                        }
                    }
                }
                if (pLenght <= 1)
                {
                    ClientManager.clients[connectionID].buffer.Clear();
                }
            }
            private static void HandleDataPackets(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                buffer.Dispose();
                if(packets.TryGetValue(packetID,out Packet packet))
                {
                    packet.Invoke(connectionID,data);
                }
            }

        }
    }
}
