using System;
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
            public static Dictionary<Guid, Pawn> pawns = new Dictionary<Guid, Pawn>();

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

                }
                //send to new client all pawn
                foreach (var pawn in pawns)
                {
                    DataSender.SendNewPawn(connectionID, pawn.Value);
                }
            }

            public static void PawnMove(Guid server_Ref, Transform pawnTransform)
            {
                if (pawns.TryGetValue(server_Ref, out Pawn pawn))
                {
                    pawn.transform = pawnTransform;
                    foreach (var client in clients)
                    {
                        // send to all new position (owner too)
                        DataSender.SendPawnMove(client.Key, server_Ref, pawnTransform);
                    }
                }
            }

            /*
            public static void PawnMove(int ID_Handler, Guid server_Ref, Transform pawnTransform)
            {
                if (clients.TryGetValue(ID_Handler, out Client clientHandler))
                {
                    Pawn target = null;
                    for (int i = 0; i < clientHandler.pawns.Count; i++)
                    {
                        if (clientHandler.pawns[i].server_Ref == server_Ref)
                        {
                            target = clientHandler.pawns[i];
                            break;
                        }
                    }

                    if (target != null)
                    {
                        target.transform = pawnTransform;
                        foreach (var client in clients)
                        {
                            // send to all new position (owner too)
                            DataSender.SendPawnMove(client.Key, ID_Handler, server_Ref, pawnTransform);
                        }
                    }
                    else
                    {
                        throw new Exception("no pawn");
                    }
                }
                else
                {
                    throw new Exception("no handler");
                }
            }
             */

            public static void NewPawn(int connectionID, Pawn newPawn)
            {
                pawns.Add(newPawn.server_Ref, newPawn);
                if (clients.TryGetValue(connectionID, out Client thisClient))
                {
                    thisClient.AssignPawn(newPawn);
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
