using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class NetworkManager : MonoBehaviour
        {
            public static NetworkManager instance;

            private void Awake()
            {
                if (instance != null)
                    Debug.LogError("Instance already assigned");
                instance = this;
                DontDestroyOnLoad(this);
            }

            private void Start()
            {
                UnityThread.initUnityThread();
                ClientHandleData.InitializePackets();
                ClientTCP.InitializeNetworking();
            }

            private void OnApplicationQuit()
            {
                ClientTCP.Disconnect();
            }
        }
    }
}