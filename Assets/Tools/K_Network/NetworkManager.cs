using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KawaSquad
{
    namespace Network
    {
        public class NetworkManager : MonoBehaviour
        {
            public static NetworkManager instance;
            public Dictionary<int, PlayerHandle> playersList = new Dictionary<int, PlayerHandle>();
            public Dictionary<Guid, PlayerController> pawns = new Dictionary<Guid, PlayerController>();

            public PlayerHandle prefabPlayer;
            public PlayerController prefabController;
            public CanvasGroup waitNetwork;

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



            public IEnumerator NetworkReady()
            {
                waitNetwork.alpha = 0f;
                waitNetwork.blocksRaycasts = false;
                waitNetwork.interactable = false;
                yield break;
            }
            public void InstantiatePlayerHandler(int index, bool isLocalClient)
            {
                PlayerHandle playerHandle = Instantiate(prefabPlayer);
                playerHandle.SetPlayerHandle(index,isLocalClient);
                playersList.Add(index, playerHandle);
            }
            public void InstantiatePawn(PlayerController.Server_PawnData data)
            {
                PlayerController playerController = Instantiate(prefabController);
                playerController.serverData = data;

                playerController.name = "Player_Controller_" + data.ID_Character;
                playerController.id_Character = data.ID_Character;
                playerController.SetPosition(data.position, data.rotation, data.scale);

                pawns.Add(data.server_Ref, playerController);
                if (playersList.TryGetValue(data.ID_Handler, out PlayerHandle handler))
                {
                    handler.AssignedPawn(playerController);
                }
                else
                {
                    Debug.LogError("Handler not created");
                }
            }

            public void AssignedPawn(int handler)
            {

            }

            public void Player_MovePawn(Guid server_Ref, Vector3 position, Vector3 rotation, Vector3 scale)
            {
                if (pawns.TryGetValue(server_Ref, out PlayerController playerTarget))
                {
                    playerTarget.SetPosition(position, rotation, scale);
                }
            }

            public void RemovePlayer(int handler)
            {
                GameObject playerCell = playersList[handler].gameObject;
                playersList.Remove(handler);
                Destroy(playerCell);
            }

            private void OnApplicationQuit()
            {
                ClientTCP.Disconnect();
            }
        }
    }
}