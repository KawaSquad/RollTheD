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
            public Dictionary<int, GameObject> playersList = new Dictionary<int, GameObject>();

            public GameObject prefabPlayer;

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

            public void InstantiatePlayer(int index)
            {
                GameObject go = Instantiate(prefabPlayer);
                go.name = "Player_" + index;
                playersList.Add(index, go);
            }

            private void OnApplicationQuit()
            {
                ClientTCP.Disconnect();
            }
        }
    }
}