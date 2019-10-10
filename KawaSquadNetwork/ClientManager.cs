using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        public class ClientManager
        {
            public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

            public static void CreateNewConnection(TcpClient tempClient)
            {
                Client newClient = new Client();
                newClient.socket = tempClient;
                newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
                newClient.Start();
                clients.Add(newClient.connectionID, newClient);

                DataSender.SendWelcomeMessage(newClient.connectionID);
                InstatiatePlayer(newClient.connectionID);
            }

            public static void InstatiatePlayer(int connectionID)
            {
                foreach (var client in clients)
                {
                    if(client.Key != connectionID)
                    {
                        DataSender.SendInstantiatePlayer(client.Key, connectionID);
                    }
                }

                foreach (var client in clients)
                {
                    DataSender.SendInstantiatePlayer(connectionID, client.Key);
                }
            }

            public static void PawnMove(int connectionID,byte[] data)
            {
                foreach (var client in clients)
                {
                    if (client.Key != connectionID)
                    {
                        DataSender.SendPawnMove(connectionID, client.Key, data);
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
