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
            public static Dictionary<Guid, Pawn> pawns = new Dictionary<Guid, Pawn>();
            public static string currentMap = "";
            public static string currentMapData = "";

            public static void CreateNewConnection(TcpClient tempClient)
            {
                Client newClient = new Client();
                newClient.socket = tempClient;
                newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
                newClient.Start();
                Debug.Log("New connection : " + newClient.connectionID);

                if (clients.Count == 0)
                    newClient.connectionID = ServerTCP.PORT;
                clients.Add(newClient.connectionID, newClient);

                if (newClient.socket.Connected)
                {
                    DataSender.SendWelcomeMessage(newClient.connectionID);
                    InstatiatePlayer(newClient.connectionID);
                }
            }

            public static void InstatiatePlayer(int connectionID)
            {
                if (!clients.ContainsKey(connectionID))
                    return;

                //send to new client all existing player handle
                try
                {
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
                    if (currentMap != "" && currentMapData != "")
                        DataSender.SendLoadMap(connectionID, currentMap, currentMapData);
                }
                catch (Exception)
                {
                    Debug.LogError("Connection not allow : " + connectionID);
                }

            }
            public static void PawnMove(int connectionID,Guid server_Ref, Transform pawnTransform)
            {
                if (pawns.TryGetValue(server_Ref, out Pawn pawn))
                {
                    pawn.transform = pawnTransform;
                    foreach (var client in clients)
                    {
                        // send to all new position (not owner too any more)

                        if(client.Value.connectionID != connectionID)
                            DataSender.SendPawnMove(client.Key, server_Ref, pawnTransform);
                    }
                    Debug.Log("Move pawn : " + server_Ref + " to : " + pawnTransform.ToString());
                }
            }

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
                    Debug.Log("New pawn : " + newPawn.server_Ref);
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

                try
                {
                    clients[connectionID].stream.BeginWrite(bufferArray, 0, bufferArray.Length, null, null);
                }
                catch (Exception)
                {
                    Debug.LogError("Can not send data to " + connectionID);
                }
                
                buffer.Dispose();
            }

            public static void DeletePawn(Guid server_Ref)
            {
                if (pawns.TryGetValue(server_Ref, out Pawn pawn))
                {
                    foreach (var client in clients)
                    {
                        DataSender.SendDeletePawn(client.Key, server_Ref);
                    }
                    pawns.Remove(server_Ref);
                    Debug.Log("Delete pawn : " + server_Ref);
                }
                else
                {
                    throw new Exception("why ?");
                }
            }
            public static void LoadMap(int connectionID, string mapPath, string dataPath)
            {
                currentMap = mapPath;
                currentMapData = dataPath;
                foreach (var client in clients)
                {
                    //connectionID already got it
                    if(client.Key != connectionID)
                        DataSender.SendLoadMap(client.Key, mapPath, dataPath);
                }
                Debug.Log("Load map : " + mapPath, true);
            }
        }
    }
}
