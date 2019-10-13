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
            public PlayerController prefabController;

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



            public void InstantiatePlayerHandler(int index, bool isLocalClient)
            {
                PlayerHandle playerHandle = Instantiate(prefabPlayer);
                playerHandle.SetPlayerHandle(index,isLocalClient);
                playersList.Add(index, playerHandle);
            }
            public void InstantiatePlayerController(int connectionID, Content_Lobby character)
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

            public void AssignedPawn(int index)
            {

            }

            public void Player_MovePawn(int index, int id_character, Vector3 position)
            {
                if (playersList.TryGetValue(index,out PlayerHandle playerTarget))
                {
                    for (int i = 0; i < playerTarget.pawns.Count; i++)
                    {
                        if (playerTarget.pawns[i].id_Character == id_character)
                        {
                            playerTarget.pawns[i].SetPosition(position, false);
                            break;
                        }
                    }

                    Debug.LogError("No pawn");
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