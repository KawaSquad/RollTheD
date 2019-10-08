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
            public Dictionary<int, PlayerHandle> playersList = new Dictionary<int, PlayerHandle>();

            public PlayerHandle prefabPlayer;

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
                PlayerHandle playerHandle = Instantiate(prefabPlayer);
                playerHandle.name = "Player_" + index;
                playersList.Add(index, playerHandle);
            }
            public void RemovePlayer(int index)
            {
                GameObject playerCell = playersList[index].gameObject;
                playersList.Remove(index);
                Destroy(playerCell);
            }

            private void OnApplicationQuit()
            {
                ClientTCP.Disconnect();
            }
        }
    }
}