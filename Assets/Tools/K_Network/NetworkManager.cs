﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
            public Dictionary<Guid, Pawn> pawns = new Dictionary<Guid, Pawn>();

            public PlayerHandle prefabPlayer;
            public CanvasGroup waitNetwork;
            public SObject_Player sessionData;

            public List<Pawn> prefabsPawn;

            private Coroutine coKeepAlive;

            private static float pingTime = 0f;
            private static float pongTime = 0f;
            private static bool waitingPing = false;

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
                InitializeNetwork();
                //ClientTCP.InitializeNetworking();
            }

            void InitializeNetwork()
            {
                if (coKeepAlive != null)
                    StopCoroutine(coKeepAlive);
                coKeepAlive = StartCoroutine(KeepAlive());

                waitNetwork.alpha = 1f;
                waitNetwork.blocksRaycasts = true;
                waitNetwork.interactable = true;

                ClientTCP.InitializeNetworking(sessionData.IP_Session, sessionData.Port_Session);
            }

            IEnumerator KeepAlive()
            {
                //Start as not connected
                while (ClientTCP.IsConnected == false)
                {
                    yield return 0;
                }

                while (ClientTCP.IsConnected)
                {
                    yield return 0;
                }
                ClientTCP.Disconnect();
                ServerQuit();
                InitializeNetwork();
                yield break;
            }

            public IEnumerator NetworkReady()
            {
                waitNetwork.alpha = 0f;
                waitNetwork.blocksRaycasts = false;
                waitNetwork.interactable = false;
                yield break;
            }

            #region Server
            private void ServerQuit()
            {
                foreach (var player in playersList)
                {
                    Destroy(player.Value.gameObject);
                }
                playersList.Clear();
                foreach (var pawn in pawns)
                {
                    Destroy(pawn.Value.gameObject);
                }
                pawns.Clear();
            }
            private void OnApplicationQuit()
            {
                ClientTCP.Disconnect();
            }
            public static void PingServer()
            {
                if (waitingPing)
                    return;

                pingTime = Time.time;
                waitingPing = true;
                DataSender.SendPingServer();
            }
            public static void HanldePing()
            {
                waitingPing = false;
                pongTime = Time.time;
                Debug.Log("Ping : " + (pongTime - pingTime) + "ms");
            }
            #endregion

            #region PlayerHandler
            public void InstantiatePlayerHandler(int index, bool isLocalClient)
            {
                PlayerHandle playerHandle = Instantiate(prefabPlayer);
                playerHandle.SetPlayerHandle(index, isLocalClient);
                playersList.Add(index, playerHandle);
            }
            public void RemovePlayer(int handler)
            {
                GameObject playerCell = playersList[handler].gameObject;
                playersList.Remove(handler);
                Destroy(playerCell);
            }
            #endregion 

            #region Pawn
            public void InstantiatePawn(Pawn.Server_PawnData data)
            {
                Pawn targetPawn = null;
                for (int i = 0; i < prefabsPawn.Count; i++)
                {
                    if (prefabsPawn[i].pawnType == data.pawnType)
                    {
                        targetPawn = prefabsPawn[i];
                        break;
                    }
                }
                if (targetPawn != null)
                {
                    Pawn newPawn = Instantiate(targetPawn);
                    newPawn.serverData = data;
                    newPawn.SetPosition(data.position, data.rotation, data.scale);

                    pawns.Add(data.server_Ref, newPawn);
                    if (playersList.TryGetValue(data.ID_Handler, out PlayerHandle handler))
                    {
                        handler.AssignedPawn(newPawn);
                    }
                    else
                    {
                        Debug.LogError("Handler not created : " + data.ID_Handler);
                    }
                }
                else
                {
                    Debug.LogError("Pawn type not listed");
                }
            }
            public void AssignedPawn(int handler)
            {

            }
            public void Player_MovePawn(Guid server_Ref, Vector3 position, Vector3 rotation, Vector3 scale)
            {
                if (pawns.TryGetValue(server_Ref, out Pawn pawnTarget))
                {
                    pawnTarget.SetPosition(position, rotation, scale);
                }
            }
            public void Player_DeletePawn(Guid server_Ref)
            {
                if (pawns.TryGetValue(server_Ref, out Pawn pawnTarget))
                {
                    Destroy(pawnTarget.gameObject);
                }
            }
            #endregion 

            #region Map
            #endregion 
        }
    }
}