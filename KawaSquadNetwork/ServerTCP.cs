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
            public static TcpListener serverSocket = new TcpListener(IPAddress.Any, 5557);

            public static void InitailizeNetWork()
            {
                Console.WriteLine("Initialize network ...");
                ServerHandleData.InitializePackets();
                serverSocket.Start();
                serverSocket.BeginAcceptSocket(new AsyncCallback(OnClientConnect), true);
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
