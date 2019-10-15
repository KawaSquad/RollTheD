﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class ClientManager
        {
            public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
            //public static Dictionary<int, Pawn> pawns = new Dictionary<int, Pawn>();

            public static void CreateNewConnection(TcpClient tempClient)
            {
                Client newClient = new Client();
                newClient.socket = tempClient;
                newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
                newClient.Start();

                if(clients.Count == 0)
                    newClient.connectionID = ServerTCP.PORT;
                clients.Add(newClient.connectionID, newClient);

                DataSender.SendWelcomeMessage(newClient.connectionID);
                InstatiatePlayer(newClient.connectionID);
            }

            public static void InstatiatePlayer(int connectionID)
            {
                //send to new client all existing player handle
                foreach (var client in clients)
                {
                    bool isNewClient = (client.Key == connectionID);
                    if (!isNewClient)
                    {
                        DataSender.SendInstantiatePlayer(client.Key, connectionID);
                    }

                    //send to existing player to the new client
                    DataSender.SendInstantiatePlayer(connectionID, client.Key, isNewClient);

                    //send to new client all pawn
                    foreach (var pawn in client.Value.pawns)
                    {
                        DataSender.SendNewPawn(connectionID, pawn);
                    }
                }
            }

            public static void PawnMove(int ID_Handler, Transform pawnTransform)
            {
                foreach (var client in clients)
                {
                    //if (client.Key != connectionID)
                    //{
                    //}
                    DataSender.SendPawnMove(client.Key, ID_Handler, pawnTransform);
                }
            }
            public static void NewPawn(int connectionID, Pawn newPawn)
            {
                if (clients.TryGetValue(connectionID, out Client thisClient))
                {
                    thisClient.pawns.Add(newPawn);

                    foreach (var client in clients)
                    {
                        DataSender.SendNewPawn(client.Key, newPawn);// newPawn.ID_Character, newPawn.transform);
                    }
                }
                else
                {
                    throw new Exception("why ?");
                }
            }
            public static void AssignPawn(int connectionID, byte[] data)
            {
                foreach (var client in clients)
                {
                    if (client.Key != connectionID)
                    {
                        DataSender.SendAssignPawn(connectionID, client.Key, data);
                    }
                }
            }

            public static void SendDataTo(int connectionID, byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
                buffer.WriteBytes(data);
                byte[] bufferArray = buffer.ToArray();
                clients[connectionID].stream.BeginWrite(bufferArray, 0, bufferArray.Length, null, null);
                buffer.Dispose();
            }
        }
    }
}
