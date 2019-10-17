using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class ClientTCP
        {
            public static bool IsConnected
            {
                get
                {
                    if (clientSocket == null)
                        return false;
                    return clientSocket.Connected;
                }
            }
            private static TcpClient clientSocket;
            private static NetworkStream stream;
            private static byte[] recBuffer;
            private static int sizeBuffer = 4096;

            private static string session_ip_adress = "127.0.0.1";
            private static int session_port = 5557;

            public static void InitializeNetworking(string ip_adress = "127.0.0.1", int port = 5557)
            {
                session_ip_adress = ip_adress;
                session_port = port;

                clientSocket = new TcpClient();
                clientSocket.ReceiveBufferSize = sizeBuffer;
                clientSocket.SendBufferSize = sizeBuffer;
                recBuffer = new byte[sizeBuffer * 2];
                clientSocket.BeginConnect(session_ip_adress, session_port, new AsyncCallback(ClientCallBack), clientSocket);
            }
            public static void ClientCallBack(IAsyncResult result)
            {
                if (clientSocket.Connected)
                {
                    Debug.Log("Connected");
                    clientSocket.EndConnect(result);

                    UnityThread.executeCoroutine(NetworkManager.instance.NetworkReady());
                    clientSocket.NoDelay = true;
                    stream = clientSocket.GetStream();
                    stream.BeginRead(recBuffer, 0, sizeBuffer * 2, ReceiveCallBack, null);
                }
                else
                {
                    //LOG
                    Debug.Log("Try again");
                    clientSocket.BeginConnect(session_ip_adress, session_port, new AsyncCallback(ClientCallBack), clientSocket);
                    return;
                }
            }
            public static void ReceiveCallBack(IAsyncResult result)
            {
                try
                {
                    int lenght = stream.EndRead(result);
                    if(lenght <= 0)
                    {
                        return;
                    }
                    byte[] newBytes = new byte[lenght];
                    Array.Copy(recBuffer, newBytes,lenght);
                    UnityThread.executeInFixedUpdate(() => 
                    {
                        ClientHandleData.HandleData(newBytes);
                    });
                    stream.BeginRead(recBuffer, 0, sizeBuffer * 2, ReceiveCallBack, null);
                }
                catch (Exception)
                {

                    //throw;
                }
            }

            public static void SendData(byte[] data)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
                buffer.WriteBytes(data);
                byte[] bufferArray = buffer.ToArray();
                stream.BeginWrite(bufferArray, 0, bufferArray.Length, null, null);
                buffer.Dispose();
            }
            public static void Disconnect()
            {
                clientSocket.Close();
            }
        }
    }
}