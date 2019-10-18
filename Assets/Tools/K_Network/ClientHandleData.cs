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
        public class ClientHandleData
        {
            private static ByteBuffer playerBuffer;
            public delegate void Packet(byte[] data);

            public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

            public static void InitializePackets()
            {
                packets.Add((int)ClientPackets.C_WELCOME_MESSAGE, DataReceiver.HandleWelcomeMessage);
                packets.Add((int)ClientPackets.C_CREATE_PLAYER, DataReceiver.HandleInstantiatePlayer);
                packets.Add((int)ClientPackets.C_PING_CLIENT, DataReceiver.HandlePingServer);

                packets.Add((int)ClientPackets.C_PAWN_MOVE, DataReceiver.HandlePawnMove);
                packets.Add((int)ClientPackets.C_NEW_PAWN, DataReceiver.HandleNewPawn);
                packets.Add((int)ClientPackets.C_ASSIGN_PAWN, DataReceiver.HandleAssignPawn);
                packets.Add((int)ClientPackets.C_DELETE_PAWN, DataReceiver.HandleDeletePawn);

                packets.Add((int)ClientPackets.C_LOAD_MAP, DataReceiver.HandleLoadMap);
            }
            public static void HandleData(byte[] data)
            {
                byte[] buffer = (byte[])data.Clone();
                int pLenght = 0;

                if (playerBuffer == null)
                    playerBuffer = new ByteBuffer();

                playerBuffer.WriteBytes(buffer);
                if (playerBuffer.Count == 0)
                {
                    playerBuffer.Clear();
                    return;
                }

                if (playerBuffer.Lenght >= 4)
                {
                    pLenght = playerBuffer.ReadInteger(false);
                    if (pLenght <= 0)
                    {
                        playerBuffer.Clear();
                        return;
                    }
                }

                while (pLenght > 0 && pLenght <= playerBuffer.Lenght - 4)
                {
                    if (pLenght <= playerBuffer.Lenght - 4)
                    {
                        playerBuffer.ReadInteger();
                        data = playerBuffer.ReadBytes(pLenght);
                        HandleDataPackets(data);
                    }
                    pLenght = 0;

                    if (playerBuffer.Lenght >= 4)
                    {
                        pLenght = playerBuffer.ReadInteger(false);
                        if (pLenght <= 0)
                        {
                            playerBuffer.Clear();
                            return;
                        }
                    }
                }
                if (pLenght <= 1)
                {
                    playerBuffer.Clear();
                }
            }
            private static void HandleDataPackets(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteBytes(data);
                int packetID = buffer.ReadInteger();
                buffer.Dispose();
                if (packets.TryGetValue(packetID, out Packet packet))
                {
                    packet.Invoke(data);
                }
            }
        }
    }
}