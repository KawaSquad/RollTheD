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
        public static class ServerTCP
        {
            public const int PORT = 5557;
            public static TcpListener serverSocket = new TcpListener(IPAddress.Any, PORT);

            public static void InitailizeNetWork()
            {
                Debug.Log("Initialize network ...", true);
                ServerHandleData.InitializePackets();

                //Create Server

                serverSocket.Start();
                serverSocket.BeginAcceptSocket(new AsyncCallback(OnClientConnect), true);

                TcpClient tcpClient = new TcpClient("127.0.0.1", PORT);
            }

            private static void OnClientConnect(IAsyncResult result)
            {
                TcpClient client = serverSocket.EndAcceptTcpClient(result);
                serverSocket.BeginAcceptSocket(new AsyncCallback(OnClientConnect), true);
                ClientManager.CreateNewConnection(client);
            }
        }
    }
}
