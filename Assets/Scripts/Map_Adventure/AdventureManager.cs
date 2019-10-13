using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class AdventureManager : MonoBehaviour
{
    public MapLoader mapLoader;
    public Camera mainCamera;

    PlayerController playerSelected = null;

    [Header("GameMaster")]
    public SObject_Player sessionData;
    public Transform listCharacters;


    [System.Serializable]
    public class ColorSelection
    {
        public Color unselected = Color.clear;
        public Color highlightedLocal = Color.blue;//Mine
        public Color highlightedServer = Color.red;//Them
        public Color active = Color.green;
    }
    public ColorSelection colorSelections;
    public static AdventureManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Debug.Log("Instance already assigned");
        Instance = this;
    }

    /*
    void Start()
    {
        mapLoader.LoadMap(sessionData.GM_Url, false);
    }
     */

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.tag == "Player")
            {
                PlayerController player = hitInfo.collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    if (playerSelected != null && playerSelected != player)
                    {
                        playerSelected.UnselectCharacter();
                        playerSelected = null;
                    }

                    player.SelectCharacter();
                    playerSelected = player;

                    if (Input.GetMouseButtonDown(0))
                    {
                        player.ActiveCharacter();
                    }
                }
            }
            else if (hitInfo.collider.tag == "Ground")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (PlayerController.activeController != null)
                    {
                        PlayerController.activeController.SetPosition(hitInfo.point, true);
                    }
                }
            }
        }
        else
        {
            if (playerSelected != null)
            {
                playerSelected.UnselectCharacter();
                playerSelected = null;
            }
        }
    }

    public void CreateCharacter(int connectionID, int id_Character, bool sendToServer)
    {
        Content_Lobby character = null;
        for (int i = 0; i < sessionData.Content_Lobby.characters.Count; i++)
        {
            if (sessionData.Content_Lobby.characters[i].ID_Character == id_Character)
                character = sessionData.Content_Lobby.characters[i];
        }
        if (character != null)
        {
            NetworkManager.instance.InstantiatePlayerController(connectionID, character);//PlayerHandle.LocalPlayerHandle.isLocalClient);
            if (sendToServer)
                DataSender.SendNewCharacter(connectionID, character.ID_Character);
        }
    }
}
