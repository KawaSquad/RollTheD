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
                if (playersList.TryGetValue(data.ID_Handler, out PlayerHandle handler))
                {
                    PlayerController playerController = Instantiate(prefabController);
                    playerController.serverData = data;

                    playerController.name = "Player_Controller_" + data.ID_Character;
                    playerController.id_Character = data.ID_Character;
                    playerController.SetPosition(data.position, data.rotation, data.scale);
                    handler.AssignedPawn(playerController);
                }
                else
                {
                    Debug.LogError("Handler not created");
                }
            }
            public void InstantiatePlayerController_OLD(int connectionID, Content_Lobby character)
            {
                if (playersList.TryGetValue(connectionID, out PlayerHandle handler))
                {
                    PlayerController playerController = Instantiate(prefabController);
                    playerController.name = "Player_Controller_" + character.Name_Character;
                    playerController.id_Character = character.ID_Character;
                    handler.AssignedPawn(playerController);
                }
                else
                {
                    Debug.LogError("Handler not created");
                }

                //playerController.isLocalClient = isLocalClient;
                //playersList.Add(index, playerController);
            }

            public void AssignedPawn(int handler)
            {

            }

            public void Player_MovePawn(int handler, Guid server_Ref, Vector3 position, Vector3 rotation, Vector3 scale)
            {
                if (playersList.TryGetValue(handler, out PlayerHandle playerTarget)) 
                {
                    for (int i = 0; i < playerTarget.pawns.Count; i++)
                    {
                        if (playerTarget.pawns[i].serverData.server_Ref == server_Ref)
                        {
                            playerTarget.pawns[i].SetPosition(position, rotation, scale);
                            break;
                        }
                    }
                }
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