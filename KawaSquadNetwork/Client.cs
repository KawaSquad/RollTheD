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
        public class Client
        {
            public int connectionID;
            public TcpClient socket;
            public NetworkStream stream;
            private byte[] recBuffer;
            public ByteBuffer buffer;

            public void Start()
            {
                socket.SendBufferSize = 4096;
                socket.ReceiveBufferSize = 4096;
                stream = socket.GetStream();
                stream.BeginRead(recBuffer,0,socket.ReceiveBufferSize,OnReceivedData,null);
                Console.WriteLine("Incoming connection '{0}'." , socket.Client.RemoteEndPoint.ToString());
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

            void CloseConnection()
            {
                Console.WriteLine("Connection from '{0}' has been terminated.",socket.Client.RemoteEndPoint.ToString());
                socket.Close();
            }
        }
    }
}