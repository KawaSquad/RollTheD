using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        public class Client //playerHandle
        {
            public int connectionID;
            public TcpClient socket;
            public NetworkStream stream;
            private byte[] recBuffer;
            public ByteBuffer buffer;

            List<Pawn> pawns = new List<Pawn>();

            public void Start()
            {
                int bufferSize = 4096;
                socket.SendBufferSize = bufferSize;
                socket.ReceiveBufferSize = bufferSize;
                stream = socket.GetStream();
                recBuffer = new byte[bufferSize];
                stream.BeginRead(recBuffer,0,socket.ReceiveBufferSize,OnReceivedData,null);
                Debug.Log("Incoming connection " + socket.Client.RemoteEndPoint.ToString(), true);
            }

            void OnReceivedData(IAsyncResult result)
            {
                try
                {
                    int lenght = stream.EndRead(result);

                    if(lenght<=0)
                    {
                        CloseConnection();
                        return;
                    }

                    byte[] newBytes = new byte[lenght];
                    Array.Copy(recBuffer, newBytes, lenght);
                    ServerHandleData.HandleData(connectionID, newBytes);
                    stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceivedData, null);
                }
                catch (Exception)
                {
                    CloseConnection();
                    return;
                }
            }

            public void AssignPawn(Pawn newPawn)
            {
                if (pawns == null)
                    pawns = new List<Pawn>();
                pawns.Add(newPawn);
            }


            void CloseConnection()
            {
                Debug.Log("Connection from " + socket.Client.RemoteEndPoint.ToString() + " has been terminated.", true);
                socket.Close();

                //SERVER KEEP ALL
                if(ClientManager.clients.TryGetValue(ServerTCP.PORT,out Client serverClient))
                {
                    for (int i = 0; i < this.pawns.Count; i++)
                    {
                        this.pawns[i].ID_Hanlder = ServerTCP.PORT;
                    }
                    serverClient.pawns.AddRange(this.pawns);
                }

                ClientManager.clients.Remove(this.connectionID);

                //DataSender.SendClientDisconnect(this.connectionID);
            }
        }
    }
}